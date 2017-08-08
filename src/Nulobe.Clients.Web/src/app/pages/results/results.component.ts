import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import {
  FactApiClient, Fact,
  VoteApiClient,
  FlagApiClient,
} from '../../features/api/api.swagger';

import { ResultsPathHelper } from './results-path.helper';

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

  constructor(
    private factApiClient: FactApiClient,
    private voteApiClient: VoteApiClient,
    private flagApiClient: FlagApiClient,
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

    this.tags = tags;
    this.loadFacts();
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

  private loadFacts() {
    let factsUpdated$ = this.factApiClient.list(this.tags.join(','));
    
    factsUpdated$.subscribe(facts => this._facts.next(facts));

    // Delay emitting loading = false intelligently:
    let delay = Observable.timer(500);
    Observable.combineLatest([factsUpdated$, delay])
      .subscribe(() => this._loading.next(false));
  }
}
