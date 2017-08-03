import { BrowserModule } from '@angular/platform-browser';
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MdButtonModule, MdIconModule } from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Auth0AuthService } from './auth/auth.service';
import { AuthComponent } from './auth/auth.component';
import { AuthCallbackComponent } from './auth/auth-callback/auth-callback.component';
import { AuthHttp } from './auth/auth-http.service';

import { AdminModule } from './pages/admin/admin.module';
import { HomeModule } from './pages/home/home.module';
import { ResultsModule } from './pages/results/results.module';

import { ApiModule } from './features/api/api.module';

import { NULOBE_ENV } from '../environments/environment';

export function httpFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions, authService: Auth0AuthService): Http {
  return new AuthHttp(xhrBackend, requestOptions, authService);
}

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    AuthCallbackComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,

    BrowserAnimationsModule,
    MdButtonModule,
    MdIconModule,

    AppRoutingModule,
    AdminModule,
    HomeModule,
    ResultsModule,

    ApiModule
  ],
  providers: [
    Auth0AuthService,
    {
      provide: Http,
      useFactory: httpFactory,
      deps: [XHRBackend, RequestOptions, Auth0AuthService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
