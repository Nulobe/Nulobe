import { NgModule } from '@angular/core';
import { StoreModule, combineReducers, compose, ActionReducer } from '@ngrx/store';
import { StoreRouterConnectingModule, routerReducer } from '@ngrx/router-store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { NULOBE_ENV } from '../../environments/environment';
import { AppStore } from './app-store';

const reducers = {
  router: routerReducer
}

@NgModule({
  imports: [
    StoreModule.forRoot(reducers),
    StoreRouterConnectingModule,
    StoreDevtoolsModule.instrument({
      maxAge: 25
    })
  ],
  providers: [
    AppStore
  ]
})
export class AppStateModule { }