import { Injectable } from '@angular/core';
import { RouterState } from '@angular/router';
import { Store } from '@ngrx/store';

interface AppState {
  router: RouterState
}

@Injectable()
export class AppStore {

  constructor(
    private store: Store<AppState>
  ) { }

  selectRouter = () => this.store.select(s => s.router);
}