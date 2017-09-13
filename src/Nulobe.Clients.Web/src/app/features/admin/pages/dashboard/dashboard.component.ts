import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ReplaySubject } from 'rxjs';

import { Fact, FactQuery } from '../../../../core/api';
import { FactQueryService } from '../../../../core/facts';
import { ContinuableResultsModel } from '../../../../core/abstractions';
import { FactPageProvider, FactPageOptions } from '../../components/fact-table';

@Component({
  selector: 'admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  factPageProvider: FactPageProvider;

  constructor(
    private factQueryService: FactQueryService,
    private router: Router
  ) { }

  ngOnInit() {
    this.factPageProvider = {
      getFactPage: (pageIndex: number, pageSize: number, options?: FactPageOptions): Observable<ContinuableResultsModel<Fact>> => {
        return this.factQueryService.query(<FactQuery>{
          pageSize: pageSize,
          tags: options.tags
        });
      }
    }
  }

  navigateToEdit(fact: Fact) {
    this.router.navigate([`/LOBE/admin/edit/${fact.id}`]);
  }
}
