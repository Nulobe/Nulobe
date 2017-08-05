import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';


import { Auth0AuthService } from '../../features/auth/auth.service';

@Injectable()
export class CanActivateAdmin implements CanActivate {

  constructor(
    private authService: Auth0AuthService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.authService.isAuthenticated();
  }
}


import { AdminComponent } from './admin.component';

export const routes: Routes = [
  {
    path: 'LOBE/admin',
    component: AdminComponent,
    canActivate: [CanActivateAdmin]
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [CanActivateAdmin]
})
export class AppRoutingModule { }
