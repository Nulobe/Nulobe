import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, UrlSerializer } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { Fact, VoteApiClient, FlagApiClient } from '../../core/api';
import { FactQueryService, FactQueryModel } from '../../core/facts';
import { IPermissionsResolver } from '../../core/abstractions';
import { AuthService } from '../../features/auth';
import { TagSelectorComponent } from '../../core/tags';

import { ResultsPathHelper } from './results-path.helper';
import { ExportResultsDialogService } from './pages/export-results-dialog/export-results-dialog.service';
import { ExportService, ExportTarget } from './services/export';
import { ExportNotificationService } from './services/export-notification/export-notification.service';

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
  private editingTags: string[] = [];
  private permissionsResolver: IPermissionsResolver;
  private isEditingTags = false;
  
  @ViewChild(TagSelectorComponent) tagSelector: TagSelectorComponent;

  constructor(
    private factQueryService: FactQueryService,
    private voteApiClient: VoteApiClient,
    private flagApiClient: FlagApiClient,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private urlSerializer: UrlSerializer,
    private authService: AuthService,
    private exportResultsDialogService: ExportResultsDialogService,
    private exportService: ExportService,
    private exportNotificationService: ExportNotificationService,
    private location: Location
  ) { }

  ngOnInit() {
    let { url } = this.router;

    let pathSections = url.split('?')[0].split('/');
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

    let urlTree = this.urlSerializer.parse(this.router.url);
    if (urlTree.queryParams.export) {
      let targetStr = ExportTarget[parseInt(urlTree.queryParams.export)];
      let target: ExportTarget = ExportTarget[targetStr];
      this.exportService.exportToTarget(tags, target)
        .then(result => {
          this.location.replaceState(pathSections.join('/'));
          this.exportNotificationService.notifySuccess(target, result);
        });
    }
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
    this.editingTags = [...this.tags];
    setTimeout(() => {
      this.isEditingTags = true;
      setTimeout(() => this.tagSelector.focus(), 0)
    });
  }

  cancelEditTags() {
    this.isEditingTags = false;
    this.editingTags = [];
  }

  completeEditTags() {
    this.tags = [...this.editingTags];
    this.cancelEditTags();
    this._loading.next(true);
    this.router.navigate([ResultsPathHelper.encode(this.tags)]).then(() => this.loadFacts());
  }

  search() {
    this._loading.next(true);
    this.tags.length = 0;
    this.tags.push(...this.editingTags);
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

  @HostListener('document:keyup', ['$event'])
  private host_handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isEditingTags && event.keyCode === 27) {
      this.cancelEditTags();
    }
  }

  @HostListener('document:click', ['$event'])
  private document_handleClick(event: any) {
    if (this.isEditingTags && (<Element[]>event.path).filter(e => e.tagName && e.tagName.toLowerCase() === 'app-tag-selector').length === 0) {
      this.cancelEditTags();
    }
  }
}
