import { UrlTree } from '@angular/router';

import { AuthConfig } from './auth-config';
import { AuthResult } from './auth-result';

export interface IAuthHandler {
  authConfig: AuthConfig;
  handleCallback(url: UrlTree): Promise<AuthResult>;
  getBearerToken(authResult: AuthResult): string
}