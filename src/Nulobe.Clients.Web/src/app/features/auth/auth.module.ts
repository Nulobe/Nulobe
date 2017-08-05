import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';

import { AuthComponent } from './components/auth/auth.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';

import { Auth0AuthService } from './auth.service';
import { AuthHttp, authHttpProvider } from './auth-http.service';

import { AuthService } from './services/auth/auth.service';
import { QuizletAuthHandler } from './services/auth/handlers/quizlet-auth-handler';

@NgModule({
  imports: [
    CommonModule,
    AuthRoutingModule
  ],
  declarations: [
    AuthComponent,
    AuthCallbackComponent
  ],
  providers: [
    AuthService,
    QuizletAuthHandler,

    Auth0AuthService,
    authHttpProvider
  ]
})
export class AuthModule { }
