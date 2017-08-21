import { Injectable, Inject } from '@angular/core';
import { Http, URLSearchParams, Headers } from '@angular/http';
import { Observable } from 'rxjs';

import { Fact, API_BASE_URL } from '../../../core/api';
import { PageModel } from '../../../core/abstractions';

import { FactQueryModel } from './fact-query.model';

@Injectable()
export class FactQueryService {

  constructor(
    private http: Http,
    @Inject(API_BASE_URL) private baseUrl: string
  ) { }

  query(model: FactQueryModel): Observable<PageModel<Fact>> {
    let params = new URLSearchParams();
    for (let key in model) {
      if (model[key]) {
        params.append(key, <string>model[key]);
      }
    }

    let headers = new Headers({
      'Access-Control-Expose-Headers': 'Content-Type, X-Total-Count'
    });

    return this.http.get(`${this.baseUrl}/facts`, { params, headers })
      .map(r => {
        return <PageModel<Fact>>{
          items: r.json(),
          count: parseInt(r.headers.get('X-Total-Count')),
          pageNumber: model.pageNumber
        }
      });
  }
}
