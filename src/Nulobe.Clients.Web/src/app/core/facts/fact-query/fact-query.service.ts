import { Injectable, Inject } from '@angular/core';
import { Http, URLSearchParams, Headers } from '@angular/http';
import { Observable } from 'rxjs';

import { Fact, FactQuery, API_BASE_URL } from '../../../core/api';
import { ContinuableResultsModel } from '../../../core/abstractions';

@Injectable()
export class FactQueryService {

  constructor(
    private http: Http,
    @Inject(API_BASE_URL) private baseUrl: string
  ) { }

  query(model: FactQuery): Observable<ContinuableResultsModel<Fact>> {
    let params = new URLSearchParams();
    for (let key in model) {
      if (model[key]) {
        params.append(key, <string>model[key]);
      }
    }

    let headers = new Headers({
      'Access-Control-Expose-Headers': 'Content-Type, X-Continuation-Token'
    });

    return this.http.get(`${this.baseUrl}/facts`, { params, headers })
      .map(r => {
        return <ContinuableResultsModel<Fact>>{
          results: r.json(),
          token: r.headers.get('X-Continuation-Token')
        }
      });
  }
}
