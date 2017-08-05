import { Injectable } from '@angular/core';
import { Http, RequestOptionsArgs, Headers } from '@angular/http';
import { UrlTree } from '@angular/router';

import { AuthConfig } from '../auth-config';
import { AuthResult } from '../auth-result';
import { IAuthHandler } from '../auth-handler';

@Injectable()
export class QuizletAuthHandler implements IAuthHandler {

  constructor(
    private http: Http
  ) { }
    
  authConfig: AuthConfig = {
    clientId: 'rYgrZDAwnj',
    domain: 'quizlet.com',
    responseType: 'code',
    scope: 'write_set',
  };

  handleCallback(currentUrl: UrlTree): Promise<AuthResult> {
    let url = `https://api.${this.authConfig.domain}/oauth/token`;

    let body = {
      grant_type: 'authorization_code',
      code: currentUrl.queryParams['code'],
      redirect_uri: 'http://localhost:5001/LOBE/callback'
    };

    let options: RequestOptionsArgs = {
      headers: new Headers({
        'Authorization': `Basic cllnclpEQXduajpKV2VoVHFaNER5NVM3RnNSZ3U0d1hy`,
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
      }),
    };

    let bodyString = Object.keys(body)
      .map(k => `${k}=${body[k]}`)
      .join('&')

    for (let key in Object.keys(body)) {

    }

    return this.http
      .post(url, bodyString, options)
      .map(result => {
        debugger;
        return <AuthResult>{};
      })
      .toPromise();
  }

}