import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthModule, AuthService, AuthGuard } from '../../features/auth';

import { AdminComponent } from './admin.component';
import { CreateFactComponent } from './pages/create-fact/create-fact.component';
import { EditFactComponent } from './pages/edit-fact/edit-fact.component';
import { EditFactResolve } from './pages/edit-fact/edit-fact.resolve';

export const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'create',
        component: CreateFactComponent
      },
      {
        path: 'edit/:factId',
        component: EditFactComponent,
        resolve: {
          fact: EditFactResolve
        }
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
