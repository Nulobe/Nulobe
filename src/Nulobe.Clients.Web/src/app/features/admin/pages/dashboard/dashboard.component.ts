import { Component, OnInit } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';

import { Fact } from '../../../../core/api';
import { FactQueryService, FactQueryModel } from '../../../../core/facts';
import { PageModel } from '../../../../core/abstractions';
import { FactPageProvider } from '../../components/fact-table';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  private factPageProvider: FactPageProvider;

  constructor(
    private factQueryService: FactQueryService
  ) { }

  ngOnInit() {
    this.factPageProvider = {
      getFactPage: (pageIndex: number, pageSize: number): Observable<PageModel<Fact>> => {
        return this.factQueryService.query({
          pageNumber: pageIndex + 1,
          pageSize: pageSize
        });
      }
    }
  }
}
