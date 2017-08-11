import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';

interface AdminState {
//   router: RouterState
}

@Injectable()
export class AdminStore {

  constructor(
    private store: Store<AdminState>
  ) { }

//   selectRouter = () => this.store.select(s => s.router);
}