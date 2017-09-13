import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { FactQueryService } from '../../core/facts';
import { Fact, FactQuery } from '../../core/api';

@Component({
  selector: 'app-fact',
  templateUrl: './fact.component.html',
  styleUrls: ['./fact.component.scss']
})
export class FactComponent implements OnInit {

  facts$: Observable<Fact[]>;

  constructor(
    private factQueryService: FactQueryService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.facts$ = this.activatedRoute.params
      .flatMap(params => this.factQueryService.query(<FactQuery>{
          slug: `${params.slugNuance}-${params.slugTitle}`
      }))
      .do(r => {
        if (r.results.length === 0) {
          this.router.navigate(['404']);
        }
      })
      .map(r => r.results);
  }
}
