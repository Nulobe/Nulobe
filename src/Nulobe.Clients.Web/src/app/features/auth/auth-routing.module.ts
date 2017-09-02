import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CoreModule } from '../../core';

import { LoginComponent } from './pages/login/login.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';

const routes: Routes = [
  {
    path: 'LOBE/login',
    component: LoginComponent
  },
  {
    path: 'LOBE/login/:authorityName',
    component: LoginComponent
  },
  {
    path: 'LOBE/callback',
    component: LoginCallbackComponent
  },
  {
    path: 'LOBE/callback/:authorityName',
    component: LoginCallbackComponent
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
