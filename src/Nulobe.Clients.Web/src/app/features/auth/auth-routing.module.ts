import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CoreModule } from '../core/core.module';
import { DebounceNavigationGuard } from '../core/debounce-navigation/debounce-navigation.guard';

import { AuthComponent } from './components/auth/auth.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';

const routes: Routes = [
  {
    path: 'LOBE/login',
    component: AuthComponent
  },
  {
    path: 'LOBE/login/:authorityName',
    component: AuthComponent
  },
  {
    path: 'LOBE/callback',
    component: AuthCallbackComponent,
    canDeactivate: [DebounceNavigationGuard],
    data: {
      debounceNavigationTime: 5000
    }
  },
  {
    path: 'LOBE/callback/:authorityName',
    component: AuthCallbackComponent,
    canDeactivate: [DebounceNavigationGuard],
    data: {
      debounceNavigationTime: 5000 
    }
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CoreModule
  ],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
