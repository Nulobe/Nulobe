import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { Fact, VoteApiClient, FlagApiClient } from '../../core/api';

@Component({
  selector: 'app-fact-list',
  templateUrl: './fact-list.component.html',
  styleUrls: ['./fact-list.component.scss']
})
export class FactListComponent implements OnInit {
  @Input() facts$: Observable<Fact>;

  constructor(
    private voteApiClient: VoteApiClient,
    private flagApiClient: FlagApiClient,
    private router: Router
  ) { }

  ngOnInit() {
  }

  navigateToTag(tag: string) {
    this.router.navigate([`q/${tag}/force`]);
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

}
