import { Injectable } from '@angular/core';
import { UrlTree } from '@angular/router';

import { NULOBE_ENV_SETTINGS } from '../../../../environments/environment';

import { AuthConfig } from '../auth-config';
import { AuthResult } from '../auth-result';
import { IAuthHandler } from '../auth-handler';

import { QuizletApiClient } from '../../api/api.swagger';

@Injectable()
export class QuizletAuthHandler implements IAuthHandler {

  constructor(
    private quizletApiClient: QuizletApiClient
  ) { }
    
  authConfig: AuthConfig = {
    clientId: NULOBE_ENV_SETTINGS.auth.quizlet.clientId,
    domain: NULOBE_ENV_SETTINGS.auth.quizlet.domain,
    responseType: 'code',
    scope: 'write_set',
  };

  handleCallback(parsedUrl: UrlTree): Promise<AuthResult> {
    return this.quizletApiClient
      .token({
        code: parsedUrl.queryParams['code'],
        redirectUri: `${NULOBE_ENV_SETTINGS.baseUrl}/LOBE/callback/quizlet`})
      .map(r => <AuthResult>{
        bearerToken: r.access_token,
        expiresIn: r.expires_in,
        userId: r.user_id
      })
      .toPromise();
  }
}