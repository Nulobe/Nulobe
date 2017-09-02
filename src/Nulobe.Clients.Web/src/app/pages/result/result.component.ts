import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { FactQueryService } from '../../core/facts';
import { Fact } from '../../core/api';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit {

  private facts$: Observable<Fact[]>;

  constructor(
    private factQueryService: FactQueryService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.facts$ = this.activatedRoute.params
      .flatMap(params => this.factQueryService.query({
          slug: `${params.slugNuance}-${params.slugTitle}`
      }))
      .do(r => {
        if (r.count === 0) {
          this.router.navigate(['404']);
        }
      })
      .map(r => r.items);
  }

  tagClicked(tag: string) {
    debugger;
    this.router.navigate([`q/${tag}`]);
  }
}
