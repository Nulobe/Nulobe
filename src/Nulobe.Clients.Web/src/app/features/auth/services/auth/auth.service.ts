import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';

import * as auth0 from 'auth0-js';

import { NULOBE_ENV_SETTINGS } from '../../../../../environments/environment';

import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';
import { IAuthHandler } from './auth-handler';

import { QuizletAuthHandler } from './handlers/quizlet-auth-handler';

interface AuthLocalStorageItem {
  expiresAt: number;
  bearerToken: string,
  authResult: AuthResult
}

export interface IAuthService {
  redirectToLogin(authorityName?: string);
  onLoginCallback();
  logout(authorityName?: string);
  isAuthenticated(authorityName?: string): boolean;
  getBearerToken(authorityName?: string): string;
}

@Injectable()
export class AuthService implements IAuthService {

  constructor(
    private injector: Injector
  ) { }

  redirectToLogin(authorityName?: string) {
    let authHandler = this.getAuthHandler(this.getAuthorityName(authorityName));
    let auth0 = this.createAuth0(authorityName, authHandler.authConfig);
    auth0.authorize();
  }

  onLoginCallback() {
    let router = this.injector.get(Router);
    let url = router.parseUrl(router.url);
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
        this.setAuthLocalStorageItem({
          expiresAt: (authResult.expiresIn * 1000) + new Date().getTime(),
          bearerToken: authResult.bearerToken,
          authResult
        }, authorityName);

        let redirectToUrl = this.getLocalStorageItem('previousUrl');
        if (!redirectToUrl) {
          redirectToUrl = '';
        }
        
        let router = this.injector.get(Router);
        // router.navigateByUrl(redirectToUrl);
        router.navigate(['']);
      });
  }

  logout(authorityName?: string) {
    throw new Error("Method not implemented.");
  }

  isAuthenticated(authorityName?: string): boolean {
    let authInfo = this.getAuthLocalStorageItem(authorityName);
    return authInfo ? new Date().getTime() < authInfo.expiresAt : false;
  }

  getBearerToken(authorityName?: string): string {
    let authInfo = this.getAuthLocalStorageItem(authorityName);
    return authInfo ? authInfo.bearerToken : null;
  }

  private createAuth0(authorityName: string, config: AuthConfig) {
    let state = `${authorityName}-${new Date().getTime()}`;
    localStorage.setItem('auth:state', state);

    let router = this.injector.get(Router);
    localStorage.setItem('auth:previousUrl', router.url);

    return new auth0.WebAuth({
      clientID: config.clientId,
      domain: config.domain,
      responseType: config.responseType,
      audience: config.audience,
      redirectUri: `${NULOBE_ENV_SETTINGS.baseUrl}/LOBE/callback`,
      scope: config.scope,
      state
    })
  }

  private getAuthHandler(authorityName: string): IAuthHandler {
    switch (authorityName.toLowerCase()) {
      case 'quizlet': return this.injector.get(QuizletAuthHandler);
      default: throw new Error("Not implemented.");
    }
  }

  private setAuthLocalStorageItem(authInfo: AuthLocalStorageItem, authorityName?: string) {
    this.setLocalStorageItem(JSON.stringify(authInfo), authorityName);
  }

  private getAuthLocalStorageItem(authorityName?: string): AuthLocalStorageItem {
    let json = this.getLocalStorageItem(authorityName);
    return json ? JSON.parse(json) : null;
  }

  private setLocalStorageItem(item: any, authorityName?: string) {
    return localStorage.setItem(this.getLocalStorageKey(authorityName), item);
  }

  private getLocalStorageItem(authorityName?: string): any {
    return localStorage.getItem(this.getLocalStorageKey(authorityName));
  }

  private getLocalStorageKey(authorityName?: string): string {
    return `auth:${this.getAuthorityName(authorityName)}`;
  }

  private getAuthorityName(authorityName?: string): string {
    return authorityName || 'auth0';
  }
}
