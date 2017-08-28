import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { Fact, VoteApiClient, FlagApiClient } from '../../core/api';
import { FactQueryService, FactQueryModel } from '../../core/facts';
import { IPermissionsResolver } from '../../core/abstractions';
import { AuthService } from '../../features/auth';
import { TagSelectorComponent } from '../../core/tags';

import { ResultsPathHelper } from './results-path.helper';
import { ExportResultsDialogService } from './pages/export-results-dialog/export-results-dialog.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {

  private _loading: BehaviorSubject<boolean> = new BehaviorSubject(true);
  private _facts = new BehaviorSubject<Fact[]>([]);

  private loading$: Observable<boolean> = this._loading.asObservable();
  private facts$: Observable<Fact[]> = this._facts.asObservable();
  private tags: string[] = [];
  private permissionsResolver: IPermissionsResolver;
  private isEditingTags = false;
  
  @ViewChild(TagSelectorComponent) tagSelector: TagSelectorComponent;

  constructor(
    private factQueryService: FactQueryService,
    private voteApiClient: VoteApiClient,
    private flagApiClient: FlagApiClient,
    private router: Router,
    private authService: AuthService,
    private exportResultsDialogService: ExportResultsDialogService
  ) { }

  ngOnInit() {
    let { url } = this.router;
    let pathSections = url.split('/');
    let tagsString = pathSections[pathSections.length - 1];

    let tags = ResultsPathHelper.decode(tagsString);
    if (!tags.length) {
      throw new Error('error parsing tags');
    }

    this.tags = tags;
    this.loadFacts();

    this.permissionsResolver = {
      resolve: () => this.authService.isAuthenticated() 
    };
  }

  navigateToTag(tag: string) {
    // Router doesn't refresh when ending up at same route, even when path changes
    this._loading.next(true);
    this.tags = [tag];
    this.router.navigate([tag]).then(() => this.loadFacts());
  }

  voteFact(fact: Fact) {
    this.voteApiClient.create({ factId: fact.id })
      .subscribe();
  }

  flagFact(fact: Fact) {
    this.flagApiClient.create({ factId: fact.id })
      .subscribe();
  }

  editFact(fact: Fact) {
    this.router.navigate([`/LOBE/admin/edit/${fact.id}`]);
  }

  beginEditTags() {
    this.isEditingTags = true;
    setTimeout(() => this.tagSelector.focus(), 0);
  }

  search() {
    this._loading.next(true);
    this.router.navigate([ResultsPathHelper.encode(this.tags)]).then(() => this.loadFacts());
    this.isEditingTags = false;
  }

  openExportDialog() {
    this.exportResultsDialogService.open(this.tags, this._facts.value);
  }

  private loadFacts() {
    let factsUpdated$ = this.factQueryService.query({
      tags: this.tags.join(',')
    });
    
    factsUpdated$.subscribe(factPage => this._facts.next(factPage.items));

    // Delay emitting loading = false intelligently:
    let delay = Observable.timer(500);
    Observable.combineLatest([factsUpdated$, delay])
      .subscribe(() => this._loading.next(false));
  }
}
