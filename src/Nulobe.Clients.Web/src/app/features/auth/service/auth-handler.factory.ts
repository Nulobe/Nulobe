import { Injectable, Injector } from '@angular/core';

import { IAuthHandler } from './auth-handler';
import { Auth0AuthHandler } from './auth0.auth-handler';
import { QuizletAuthHandler } from './quizlet.auth-handler';

export interface IAuthHandlerFactory {
    createAuthHandler(authorityName: string): IAuthHandler;
}

@Injectable()
export class AuthHanderFactory implements IAuthHandlerFactory {

    constructor(
        private injector: Injector
    ) { }

    createAuthHandler(authorityName: string): IAuthHandler {
        switch (authorityName.toLowerCase()) {
            case 'auth0': return this.injector.get(Auth0AuthHandler);
            case 'quizlet': return this.injector.get(QuizletAuthHandler);
            default: throw new Error("Not implemented.");
        }
    }
}
