import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';

import { AdminStore } from './admin-store';

const reducers = {
//   router: routerReducer
}

@NgModule({
  imports: [
    //StoreModule.forFeature(reducers),
  ],
  providers: [
    AdminStore
  ]
})
export class AdminStateModule { }