import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';

import { CoreModule } from '../../core';

import { AuthRoutingModule } from './auth-routing.module';

import { LoginComponent } from './pages/login/login.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';

import { AuthService } from './service/auth.service';
import { AuthHanderFactory } from './service/auth-handler.factory';
import { Auth0AuthHandler } from './service/auth0.auth-handler';
import { QuizletAuthHandler } from './service/quizlet.auth-handler';
import { AuthGuard } from './guard/auth.guard';

@NgModule({
  imports: [
    CommonModule,
    HttpModule,
    CoreModule,
    AuthRoutingModule
  ],
  declarations: [
    LoginComponent,
    LoginCallbackComponent
  ],
  providers: [
    AuthService,
    AuthHanderFactory,
    QuizletAuthHandler,
    Auth0AuthHandler,
    AuthGuard
  ]
})
export class AuthModule { }
