import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http';
import { NgModule, Injector } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MdButtonModule, MdIconModule } from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { HomeModule } from './pages/home/home.module';
import { ResultsModule } from './pages/results/results.module';

import { AppComponent } from './app.component';
import { AuthModule, AuthService } from './features/auth';
import { AuthHttp, authHttpProvider } from './auth-http.service';

export const authHttpFactory = (backend: XHRBackend, defaultOptions: RequestOptions, injector: Injector) => {
  let authService = injector.get(AuthService);
  return new AuthHttp(backend, defaultOptions, authService);
};

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule,

    BrowserAnimationsModule,
    MdButtonModule,
    MdIconModule,

    AppRoutingModule,
    HomeModule,
    ResultsModule,

    AuthModule
  ],
  providers: [
    AuthHttp,
    {
      provide: Http,
      useFactory: authHttpFactory,
      deps: [XHRBackend, RequestOptions, Injector]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
