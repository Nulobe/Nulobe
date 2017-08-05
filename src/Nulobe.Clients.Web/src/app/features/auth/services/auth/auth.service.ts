import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import * as auth0 from 'auth0-js';

import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';
import { IAuthHandler } from './auth-handler';

import { QuizletAuthHandler } from './handlers/quizlet-auth-handler';

export interface IAuthService {
  redirectToLogin(authorityName?: string);
  onLoginCallback();
  logout(authorityName?: string);
  isAuthenticated(authorityName?: string): boolean;
  getIdToken(authorityName?: string): string;
}

@Injectable()
export class AuthService implements IAuthService {

  constructor(
    private router: Router,
    private quizletAuthHandler: QuizletAuthHandler
  ) { }

  redirectToLogin(authorityName?: string) {
    let authHandler = this.getAuthHandler(this.getAuthorityName(authorityName));
    let auth0 = this.createAuth0(authorityName, authHandler.authConfig);
    auth0.authorize();
  }

  onLoginCallback() {
    let url = this.router.parseUrl(this.router.url);
    let params = url.queryParams;
    
    let state = params['state'];
    if (!state) {
      // TODO: Handle error
    }
    //TODO: Validate state vs localStorage

    let [authorityName] = state.split('-');
    let authHandler = this.getAuthHandler(authorityName);
    authHandler.handleCallback(url)
      .then(authResult => {

        const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
        this.setLocalStorageItem(authResult.accessToken, 'access_token', authorityName);
        this.setLocalStorageItem(authResult.idToken, 'id_token', authorityName);
        this.setLocalStorageItem(expiresAt, 'expires_at', authorityName);

        let redirectToUrl = this.getLocalStorageItem('previousUrl');
        if (!redirectToUrl) {
          redirectToUrl = '';
        }
        this.router.navigateByUrl(redirectToUrl);
      });
  }

  logout(authorityName?: string) {
    throw new Error("Method not implemented.");
  }

  isAuthenticated(authorityName?: string): boolean {
    const expiresAt = JSON.parse(this.getLocalStorageItem(authorityName, 'expires_at'));
    return new Date().getTime() < expiresAt;
  }

  getIdToken(authorityName?: string): string {
    return this.getLocalStorageItem(authorityName, 'id_token');
  }

  private createAuth0(authorityName: string, config: AuthConfig) {
    let state = `${authorityName}-${new Date().getTime()}`;
    localStorage.setItem('auth:state', state);
    localStorage.setItem('auth:previousUrl', this.router.url);

    return new auth0.WebAuth({
      clientID: config.clientId,
      domain: config.domain,
      responseType: config.responseType,
      audience: config.audience,
      redirectUri: 'http://localhost:5001/LOBE/callback', // TODO: Resolve from NULOBE_ENV
      scope: config.scope,
      state
    })
  }

  private getAuthHandler(authorityName: string): IAuthHandler {
    switch (authorityName.toLowerCase()) {
      case 'quizlet': return this.quizletAuthHandler;
      default: throw new Error("Not implemented.");
    }
  }

  private setLocalStorageItem(item: any, property: string, authorityName?: string) {
    return localStorage.setItem(this.getLocalStorageKey(property, authorityName), item);
  }

  private getLocalStorageItem(property: string, authorityName?: string): any {
    return localStorage.getItem(this.getLocalStorageKey(property, authorityName));
  }

  private getLocalStorageKey(property: string, authorityName?: string): string {
    return `auth:${this.getAuthorityName(authorityName)}:${property}`;
  }

  private getAuthorityName(authorityName?: string): string {
    return authorityName || 'auth0';
  }
}
