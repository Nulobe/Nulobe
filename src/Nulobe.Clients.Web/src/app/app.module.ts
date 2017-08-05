import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http';
import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MdButtonModule, MdIconModule } from '@angular/material';

import { AppComponent } from './app.component';

import { AdminModule } from './pages/admin/admin.module';
import { HomeModule } from './pages/home/home.module';
import { ResultsModule } from './pages/results/results.module';

import { AuthModule } from './features/auth/auth.module';
import { ApiModule } from './features/api/api.module';

// export function authHttpFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions, authService: Auth0AuthService): Http {
//   return new AuthHttp(xhrBackend, requestOptions, authService);
// }

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule,

    BrowserAnimationsModule,
    MdButtonModule,
    MdIconModule,

    AdminModule,
    HomeModule,
    ResultsModule,

    AuthModule,
    ApiModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
