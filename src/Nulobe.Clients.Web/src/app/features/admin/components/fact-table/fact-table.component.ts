import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { MdPaginator, PageEvent } from '@angular/material';
import { DataSource, CollectionViewer } from '@angular/cdk';
import { Observable } from 'rxjs';

import { Fact } from '../../../../core/api';
import { PageModel } from '../../../../core/abstractions';
import { FactPageProvider } from './fact-page.provider';

@Component({
  selector: 'app-fact-table',
  templateUrl: './fact-table.component.html',
  styleUrls: ['./fact-table.component.scss']
})
export class FactTableComponent implements OnInit {
  @Input() factPageProvider: FactPageProvider;
  @Output() onEdit = new EventEmitter<Fact>();

  @ViewChild(MdPaginator) paginator: MdPaginator;
  private dataSource: FactDataSource;
  private displayedColumns = ['title', 'definition', 'controls'];
  private factCount = -1;

  constructor() { }

  ngOnInit() {
    let factPageProvider = {
      getFactPage: (pageIndex: number, pageSize: number): Observable<PageModel<Fact>> => {
        let factPage$: Observable<PageModel<Fact>> = this.factPageProvider ?
          this.factPageProvider.getFactPage(pageIndex, pageSize) :
          Observable.of(<PageModel<Fact>>{
            items: [],
            count: 0,
            pageNumber: 1
          });

        return factPage$.do(p => {
          this.factCount = p.count;
        });
      }
    };

    this.dataSource = new FactDataSource(factPageProvider, this.paginator);
  }

  editFact(fact: Fact) {
    this.onEdit.emit(fact);
  }
}

class FactDataSource implements DataSource<Fact> {

  constructor(
    private factPageProvider: FactPageProvider,
    private paginator: MdPaginator) { }

  connect(collectionViewer: CollectionViewer): Observable<Fact[]> {
    let dataChanges = [
      collectionViewer.viewChange,
      this.paginator.page
    ];

    return Observable.merge(...dataChanges).flatMap(() => {
      let { pageIndex, pageSize } = this.paginator;
      return this.factPageProvider.getFactPage(pageIndex, pageSize).map(p => p.items);
    });
  }

  disconnect(collectionViewer: CollectionViewer): void { }

}
