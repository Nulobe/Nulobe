import { NULOBE_ENV } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import * as auth0 from 'auth0-js';

export interface IAuthService {
  login(): void;
  onLoginCallback(): void;
  logout(): void;
  isAuthenticated(): boolean;
  getIdToken(): string;
}

@Injectable()
export class Auth0AuthService implements IAuthService {

  // auth0 = new auth0.WebAuth({
  //   clientID: NULOBE_ENV.AUTH_CLIENT_ID,
  //   domain: NULOBE_ENV.AUTH_DOMAIN,
  //   responseType: 'token id_token',
  //   audience: `https://${NULOBE_ENV.AUTH_DOMAIN}/userinfo`,
  //   redirectUri: 'http://localhost:5001/LOBE/callback', // TODO: Resolve from NULOBE_ENV
  //   scope: 'openid'
  // });

  auth0 = new auth0.WebAuth({
    clientID: "rYgrZDAwnj",
    domain: "quizlet.com",
    responseType: 'code',
    //audience: `https://${NULOBE_ENV.AUTH_DOMAIN}/userinfo`,
    redirectUri: 'http://localhost:5001/LOBE/callback', // TODO: Resolve from NULOBE_ENV
    scope: 'write_set',
    state: 'x'
  });

  constructor() { }

  login(): void {
    this.auth0.authorize();
  }

  onLoginCallback(): void {
    this.auth0.parseHash((err, authResult) => {
      debugger;
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        
        const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt);

      } else if (err) {
        console.log(err);
        alert(`Error: ${err.error}. Check the console for further details.`);
      }
    });
  }

  logout(): void {

  }

  isAuthenticated(): boolean {
    const expiresAt = JSON.parse(localStorage.getItem('expires_at'));
    return new Date().getTime() < expiresAt;
  }

  getIdToken(): string {
    return localStorage.getItem('id_token');
  }
}
