import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';

import { ApiModule } from '../api/api.module';

import { AuthRoutingModule } from './auth-routing.module';

import { AuthComponent } from './components/auth/auth.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';

import { AuthService } from './services/auth/auth.service';
import { QuizletAuthHandler } from './services/auth/handlers/quizlet-auth-handler';

@NgModule({
  imports: [
    CommonModule,
    HttpModule,
    ApiModule,
    AuthRoutingModule
  ],
  declarations: [
    AuthComponent,
    AuthCallbackComponent
  ],
  providers: [
    AuthService,
    QuizletAuthHandler,
  ]
})
export class AuthModule { }
