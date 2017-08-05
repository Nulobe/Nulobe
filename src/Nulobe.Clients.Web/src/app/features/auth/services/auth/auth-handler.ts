import { UrlTree } from '@angular/router';

import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';

// TODO: Method for getting bearer token, I think it's different for Auth0 (id token, isntead of acccess token)
export interface IAuthHandler {
  authConfig: AuthConfig;
  handleCallback(url: UrlTree): Promise<AuthResult>; 
}