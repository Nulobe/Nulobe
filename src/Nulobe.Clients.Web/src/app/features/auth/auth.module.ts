import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';

import { CoreModule } from '../../core';

import { AuthRoutingModule } from './auth-routing.module';

import { AuthComponent } from './pages/auth/auth.component';
import { AuthCallbackComponent } from './pages/auth-callback/auth-callback.component';

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
    AuthComponent,
    AuthCallbackComponent
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
