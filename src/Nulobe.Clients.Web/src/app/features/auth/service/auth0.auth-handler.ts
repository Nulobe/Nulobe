import { Injectable } from '@angular/core';
import { UrlTree } from '@angular/router';

import { NULOBE_ENV_SETTINGS } from '../../../app.settings';

import { AUTH_CONSTANTS } from '../auth.constants';
import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';
import { IAuthHandler } from './auth-handler';
import { Auth0Helper } from './auth0.helper';

@Injectable()
export class Auth0AuthHandler implements IAuthHandler {
    
  authConfig: AuthConfig = {
    clientId: NULOBE_ENV_SETTINGS.auth.auth0.clientId,
    domain: NULOBE_ENV_SETTINGS.auth.auth0.domain,
    audience: `https://${NULOBE_ENV_SETTINGS.auth.auth0.domain}/userinfo`,
    responseType: 'token id_token',
    scope: 'open_id',
  };

  handleCallback(parsedUrl: UrlTree): Promise<AuthResult> {
    let auth0 = Auth0Helper.getAuth0(
      this.authConfig,
      AUTH_CONSTANTS.AUTHORITY_NAMES.AUTH0);

    return new Promise<AuthResult>((resolve, reject) => {
      auth0.parseHash((err, authResult) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          resolve(<AuthResult>{
            bearerToken: authResult.idToken,
            expiresIn: authResult.expiresIn,
            accessToken: authResult.accessToken
          });
        } else {
          reject(err.error);
        }
      });
    });
  }
}