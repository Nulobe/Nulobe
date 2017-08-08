import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

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
  @Output() onTagSelect = new EventEmitter<string>();
  @Output() onVote = new EventEmitter<Fact>();
  @Output() onFlag = new EventEmitter<Fact>();

  constructor() { }

  ngOnInit() {
    if (!this.factLinkResolver) {
      this.factLinkResolver = {
       resolve: (f: Fact) => '#' 
      };
    }
  }

  tagClicked(tag: string) {
    this.onTagSelect.emit(tag);
  }

  voteClicked(fact: Fact) {
    this.onVote.emit(fact);
  }

  flagClicked(fact: Fact) {
    this.onFlag.emit(fact);
  }
}
