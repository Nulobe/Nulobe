import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { MdPaginator, PageEvent } from '@angular/material';
import { DataSource, CollectionViewer } from '@angular/cdk';
import { Observable, BehaviorSubject } from 'rxjs';

import { Fact } from '../../../../core/api';
import { ContinuableResultsModel } from '../../../../core/abstractions';
import { FactPageProvider, FactPageOptions } from './fact-page.provider';

@Component({
  selector: 'admin-fact-table',
  templateUrl: './fact-table.component.html',
  styleUrls: ['./fact-table.component.scss']
})
export class FactTableComponent implements OnInit {
  @Input() factPageProvider: FactPageProvider;
  @Output() onEdit = new EventEmitter<Fact>();

  @ViewChild(MdPaginator) paginator: MdPaginator;
  dataSource: FactDataSource;
  displayedColumns = ['title', /*'definition',*/ 'tags', 'controls'];
  factCount = -1;

  constructor() { }

  ngOnInit() {
    let factPageProvider = {
      getFactPage: (pageIndex: number, pageSize: number, options?: FactPageOptions): Observable<ContinuableResultsModel<Fact>> => {
        let factPage$: Observable<ContinuableResultsModel<Fact>> = this.factPageProvider ?
          this.factPageProvider.getFactPage(pageIndex, pageSize, options) :
          Observable.of(<ContinuableResultsModel<Fact>>{
            results: []
          });

        return factPage$.do(p => {
          this.factCount = p.results.length;
        });
      }
    };

    this.dataSource = new FactDataSource(factPageProvider, this.paginator);
  }

  editFact(fact: Fact) {
    this.onEdit.emit(fact);
  }

  updateTags(tags: string[]) {
    this.dataSource.tags = tags;
  }
}

class FactDataSource implements DataSource<Fact> {
  tagsChange = new BehaviorSubject<string[]>([]);
  get tags(): string[] { return this.tagsChange.value; }
  set tags(tags: string[]) { this.tagsChange.next(tags); }

  constructor(
    private factPageProvider: FactPageProvider,
    private paginator: MdPaginator) { }

  connect(collectionViewer: CollectionViewer): Observable<Fact[]> {
    let dataChanges = [
      collectionViewer.viewChange,
      this.paginator.page,
      this.tagsChange
    ];

    return Observable.merge(...dataChanges).flatMap(() => {
      let { pageIndex, pageSize } = this.paginator;
      return this.factPageProvider.getFactPage(pageIndex, pageSize, {
        tags: this.tags.join(',')
      }).map(p => p.results);
    });
  }

  disconnect(collectionViewer: CollectionViewer): void { }

}
