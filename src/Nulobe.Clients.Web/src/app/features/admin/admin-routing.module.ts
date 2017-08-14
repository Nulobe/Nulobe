import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthModule, AuthService, AuthGuard } from '../../features/auth';

import { AdminComponent } from './admin.component';
import { CreateFactsComponent } from './pages/create-facts/create-facts.component';

export const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'create',
        component: CreateFactsComponent
      }
    ]
  }
];


@NgModule({
  imports: [
    RouterModule.forChild(routes),
    AuthModule
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
