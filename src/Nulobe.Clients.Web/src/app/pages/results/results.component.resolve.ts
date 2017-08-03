import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { FactApiClient, Fact } from '../../features/api/api.swagger';

import { ResultsPathHelper } from './results-path.helper';

@Injectable()
export class ResultsComponentResolve implements Resolve<Fact[]> {

  constructor(
      private factApiClient: FactApiClient
  ) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Fact[]> {
    debugger;

    let tagsQuery = route.queryParams['tags'];
    if (!tagsQuery) {
        return Promise.resolve([]);
    }

    let tags = ResultsPathHelper.decode(tagsQuery);
    if (!tags.length) {
        return Promise.resolve([]);
    }

    return Observable.from([]).toPromise();
  }
}