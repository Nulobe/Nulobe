import { Component, OnInit, Input } from '@angular/core';

import { Fact } from '../../api/api.swagger';

export interface FactLinkResolver {
  resolve(fact: Fact): string;
}

@Component({
  selector: 'app-fact-list',
  templateUrl: './fact-list.component.html',
  styleUrls: ['./fact-list.component.scss']
})
export class FactListComponent implements OnInit {
  @Input() facts: Fact[];
  @Input() factLinkResolver: FactLinkResolver;

  constructor() { }

  ngOnInit() {
    if (!this.factLinkResolver) {
      this.factLinkResolver = {
       resolve: (f: Fact) => '#' 
      };
    }
  }

}
