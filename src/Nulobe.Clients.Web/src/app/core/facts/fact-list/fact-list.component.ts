import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Fact } from '../../api/api.swagger';
import { IPermissionsResolver } from '../../abstractions';

export interface FactLinkResolver {
  resolve(fact: Fact | string): string;
}

@Component({
  selector: 'core-fact-list',
  templateUrl: './fact-list.component.html',
  styleUrls: ['./fact-list.component.scss']
})
export class FactListComponent implements OnInit {
  @Input() facts: Fact[];
  @Input() factLinkResolver: FactLinkResolver;
  @Input() permissionsResolver: IPermissionsResolver;
  @Output() onTagSelect = new EventEmitter<string>();
  @Output() onVote = new EventEmitter<Fact>();
  @Output() onFlag = new EventEmitter<Fact>();
  @Output() onEdit = new EventEmitter<Fact>();
  @Output() onLink = new EventEmitter<Fact>();

  private canEdit: boolean = false;

  constructor() { }

  ngOnInit() {
    if (!this.factLinkResolver) {
      this.factLinkResolver = {
        resolve: (f: Fact) => '#' 
      };
    }

    if (this.permissionsResolver) {
      this.canEdit = this.permissionsResolver.resolve('fact', 'edit');
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

  linkClicked(fact: Fact) {
    this.onLink.emit(fact);
  }

  editClicked(fact: Fact) {
    this.onEdit.emit(fact);
  }
}
