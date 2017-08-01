import { NULOBE_ENV } from '../../environments/environment';
import { Injectable } from '@angular/core';
import * as auth0 from 'auth0-js';

@Injectable()
export class AuthService {

  auth0 = new auth0.WebAuth({
    clientID: NULOBE_ENV.AUTH_CLIENT_ID,
    domain: NULOBE_ENV.AUTH_DOMAIN,
    responseType: 'token id_token',
    audience: `https://${NULOBE_ENV.AUTH_DOMAIN}/userinfo`,
    redirectUri: 'http://localhost:5001/callback', // TODO: Resolve from NULOBE_ENV
    scope: 'openid'
  });

  constructor() { }

  login() {
    this.auth0.authorize();
  }
}
