import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { FactCreate } from '../../api/api.swagger';

export interface FactLinkResolver {
  resolve(fact: FactCreate): string;
}

@Component({
  selector: 'app-fact-list',
  templateUrl: './fact-list.component.html',
  styleUrls: ['./fact-list.component.scss']
})
export class FactListComponent implements OnInit {
  @Input() facts: FactCreate[];
  @Input() factLinkResolver: FactLinkResolver;
  @Output() onTagSelect = new EventEmitter<string>();
  @Output() onVote = new EventEmitter<FactCreate>();
  @Output() onFlag = new EventEmitter<FactCreate>();

  constructor() { }

  ngOnInit() {
    if (!this.factLinkResolver) {
      this.factLinkResolver = {
       resolve: (f: FactCreate) => '#' 
      };
    }
  }

  tagClicked(tag: string) {
    this.onTagSelect.emit(tag);
  }

  voteClicked(fact: FactCreate) {
    this.onVote.emit(fact);
  }

  flagClicked(fact: FactCreate) {
    this.onFlag.emit(fact);
  }
}
