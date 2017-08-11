import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';

import { CoreModule } from '../../core';

import { AuthRoutingModule } from './auth-routing.module';

import { AuthComponent } from './components/auth/auth.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';

import { AuthService } from './auth.service';
import { AuthHanderFactory } from './auth-handler.factory';
import { Auth0AuthHandler } from './auth-handlers/auth0.auth-handler';
import { QuizletAuthHandler } from './auth-handlers/quizlet.auth-handler';

@NgModule({
  imports: [
    CommonModule,
    HttpModule,
    CoreModule,
    AuthRoutingModule
  ],
  declarations: [
    AuthComponent,
    AuthCallbackComponent
  ],
  providers: [
    AuthService,
    AuthHanderFactory,
    QuizletAuthHandler,
    Auth0AuthHandler
  ]
})
export class AuthModule { }
