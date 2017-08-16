import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { Fact, FactApiClient } from '../../../../core/api';

@Injectable()
export class EditFactResolve implements Resolve<Fact> {

  constructor(
    private factApiClient: FactApiClient
  ) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Fact> {
    let factId: string = route.paramMap.get('factId');
    return this.factApiClient.get(factId).toPromise();
  }
}