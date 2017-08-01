import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MdButtonModule, MdIconModule } from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Auth0AuthService } from './auth/auth.service';
import { AuthComponent } from './auth/auth.component';
import { AuthCallbackComponent } from './auth/auth-callback/auth-callback.component';

import { AdminModule } from './pages/admin/admin.module';
import { HomeModule } from './pages/home/home.module';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    AuthCallbackComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    MdButtonModule,
    MdIconModule,

    AdminModule,
    HomeModule
  ],
  providers: [
    Auth0AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
