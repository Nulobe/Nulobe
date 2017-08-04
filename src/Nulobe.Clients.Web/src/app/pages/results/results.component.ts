import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { FactApiClient, Fact } from '../../features/api/api.swagger';

import { ResultsPathHelper } from './results-path.helper';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {

  private _loading: BehaviorSubject<boolean> = new BehaviorSubject(true);

  private facts$: Observable<Fact[]>;
  private loading$: Observable<boolean> = this._loading.asObservable();

  constructor(
    private factApiClient: FactApiClient,
    private router: Router
  ) { }

  ngOnInit() {
    let { url } = this.router;
    let pathSections = url.split('/');
    let tagsString = pathSections[pathSections.length - 1];

    let tags = ResultsPathHelper.decode(tagsString);
    if (!tags.length) {
      throw new Error('error parsing tags');
    }

    this.loadFacts(tags);
  }

  navigateToTag(tag: string) {
    // Router doesn't refresh when ending up at same route, even when path changes
    this._loading.next(true);
    this.router.navigate([tag])
      .then(() => this.loadFacts([tag]));
  }

  private loadFacts(tags: string[]) {
    this.facts$ = this.factApiClient.list(tags.join(','));

    // Delay emitting loading = false intelligently:
    let delay = Observable.timer(500);
    Observable.combineLatest([this.facts$, delay])
      .subscribe(() => this._loading.next(false));
  }
}
