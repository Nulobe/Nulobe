import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ReplaySubject } from 'rxjs';

import { Fact } from '../../../../core/api';
import { FactQueryService, FactQueryModel } from '../../../../core/facts';
import { PageModel } from '../../../../core/abstractions';
import { FactPageProvider, FactPageOptions } from '../../components/fact-table';

@Component({
  selector: 'admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  private factPageProvider: FactPageProvider;

  constructor(
    private factQueryService: FactQueryService,
    private router: Router
  ) { }

  ngOnInit() {
    this.factPageProvider = {
      getFactPage: (pageIndex: number, pageSize: number, options?: FactPageOptions): Observable<PageModel<Fact>> => {
        return this.factQueryService.query({
          pageNumber: pageIndex + 1,
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
