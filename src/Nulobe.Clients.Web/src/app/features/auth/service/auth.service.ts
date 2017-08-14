import { Injectable, Injector } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { NULOBE_ENV_SETTINGS } from '../../../../environments/environment';

import { AUTH_CONSTANTS } from './../auth.constants';
import { AuthHanderFactory } from './auth-handler.factory';
import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';
import { IAuthHandler } from './auth-handler';
import { Auth0Helper } from './auth0.helper';

interface AuthLocalStorageItem {
  expiresAt: number;
  bearerToken: string,
  authResult: AuthResult
}

export interface IAuthService {
  redirectToLogin(authorityName?: string);
  onLoginCallback(): Promise<void>;
  logout(authorityName?: string);
  isAuthenticated(authorityName?: string): boolean;
  getBearerToken(authorityName?: string): string;
}

@Injectable()
export class AuthService implements IAuthService {

  constructor(
    private authHanderFactory: AuthHanderFactory,
    private injector: Injector
  ) { }

  redirectToLogin(authorityName?: string) {
    let resolvedAuthorityName = this.getAuthorityName(authorityName);
    let authHandler = this.authHanderFactory.createAuthHandler(resolvedAuthorityName);

    let state = `${resolvedAuthorityName}-${new Date().getTime()}`;
    localStorage.setItem('auth:state', state);

    let auth0 = Auth0Helper.getAuth0(authHandler.authConfig, resolvedAuthorityName, state);
    auth0.authorize();
  }

  onLoginCallback(authorityName?: string): Promise<void> {
    let authHandler = this.authHanderFactory.createAuthHandler(this.getAuthorityName(authorityName));

    let router = this.injector.get(Router);
    let url = router.parseUrl(router.url);

    return authHandler.handleCallback(url)
      .then(authResult => {
        this.setAuthLocalStorageItem({
          expiresAt: (authResult.expiresIn * 1000) + new Date().getTime(),
          bearerToken: authResult.bearerToken,
          authResult
        }, authorityName);
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
    return authorityName || AUTH_CONSTANTS.AUTHORITY_NAMES.AUTH0;
  }
}
