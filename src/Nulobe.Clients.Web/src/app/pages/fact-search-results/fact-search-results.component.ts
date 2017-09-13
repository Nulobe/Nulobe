import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Location } from '@angular/common';
import { Router, UrlSerializer, NavigationStart, NavigationEnd } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { Fact, FactQuery } from '../../core/api';
import { FactQueryService } from '../../core/facts';
import { IPermissionsResolver, PageModel } from '../../core/abstractions';
import { AuthService } from '../../features/auth';
import { TagSelectorComponent } from '../../core/tags';

import { TagEncodingHelper } from './helpers';
import { ExportResultsDialogService } from './pages/export-results-dialog/export-results-dialog.service';
import { ExportService, ExportTarget } from './services/export';
import { ExportNotificationService } from './services/export-notification/export-notification.service';

@Component({
  selector: 'app-fact-search-results',
  templateUrl: './fact-search-results.component.html',
  styleUrls: ['./fact-search-results.component.scss']
})
export class FactSearchResultsComponent implements OnInit {

  private _loading: BehaviorSubject<boolean> = new BehaviorSubject(true);
  private _facts = new BehaviorSubject<Fact[]>([]);
  
  loading$: Observable<boolean> = this._loading.asObservable();

  private facts$: Observable<Fact[]> = this._facts.asObservable();
  private tags: string[] = [];
  private editingTags: string[] = [];
  
  private isEditingTags = false;
  
  @ViewChild(TagSelectorComponent) tagSelector: TagSelectorComponent;

  constructor(
    private factQueryService: FactQueryService,
    private router: Router,
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

    let tags = TagEncodingHelper.decode(tagsString);
    if (!tags.length) {
      throw new Error('error parsing tags');
    }

    this.tags = tags;
    this.loadFacts();

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

    this.router.events.subscribe(e => {
      if (e instanceof NavigationStart || e instanceof NavigationEnd) {
        let urlSplit = e.url.split('/');
        if (urlSplit[1] === 'q') {
          if (e instanceof NavigationStart) {
            this.tags = TagEncodingHelper.decode(urlSplit[2]);
            this._loading.next(true);
          } else if (e instanceof NavigationEnd) {
            this.loadFacts();
          }
        }
      }
    });
  }

  navigateToTag(tag: string) {
    this.router.navigate([`q/${tag}/force`]);
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

    if (this.tags.length) {
      this.router.navigate([`q/${TagEncodingHelper.encode(this.tags)}/force`]);
    }
  }

  areTagsEdited() {
    let { tags, editingTags } = this;

    if (tags.length !== editingTags.length) {
      return true;
    }

    for (let i = 0; i < tags.length; i++) {
      if (tags[i] !== editingTags[i]) {
        return true;
      }
    }

    return false;
  }

  openExportDialog() {
    this.exportResultsDialogService.open(this.tags, this._facts.value);
  }

  private loadFacts() {
    let factsUpdated$ = this.factQueryService.query(<FactQuery>{
      tags: this.tags.join(',')
    }).share();
    
    factsUpdated$.subscribe(factPage => {
      this._facts.next(factPage.results);
    });

    // Delay emitting loading = false intelligently:
    let delay = Observable.timer(500);
    Observable.combineLatest([factsUpdated$, delay])
      .subscribe(([factPage]) => {
        this._loading.next(false);

        if (factPage.results.length === 0) {
          this.beginEditTags();
        }
      });
  }

  @HostListener('document:keyup', ['$event'])
  private host_handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isEditingTags && event.keyCode === 27) {
      this.cancelEditTags();
    }
  }

  @HostListener('document:click', ['$event'])
  private document_handleClick(event: any) {
    if (this.isEditingTags && (<Element[]>event.path).filter(e => e.tagName && e.tagName.toLowerCase() === 'core-tag-selector').length === 0) {
      this.cancelEditTags();
    }
  }
}
