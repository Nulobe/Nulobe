import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from '../../features/auth';

@Injectable()
export class CanActivateAdmin implements CanActivate {

  constructor(
    private authService: AuthService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.authService.isAuthenticated();
  }
}


import { AdminComponent } from './admin.component';
import { CreateFactsComponent } from './pages/create-facts/create-facts.component';

export const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [CanActivateAdmin],
    children: [
      {
        path: 'create',
        component: CreateFactsComponent
      }
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [CanActivateAdmin]
})
export class AdminRoutingModule { }
