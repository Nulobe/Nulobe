import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { FactCreate } from '../../api/api.swagger';
import { IPermissionsResolver } from '../../abstractions';

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
  @Input() permissionsResolver: IPermissionsResolver;
  @Output() onTagSelect = new EventEmitter<string>();
  @Output() onVote = new EventEmitter<FactCreate>();
  @Output() onFlag = new EventEmitter<FactCreate>();
  @Output() onEdit = new EventEmitter<FactCreate>();

  private canEdit: boolean = false;

  constructor() { }

  ngOnInit() {
    if (!this.factLinkResolver) {
      this.factLinkResolver = {
       resolve: (f: FactCreate) => '#' 
      };
    }

    if (this.permissionsResolver) {
      this.canEdit = this.permissionsResolver.resolve('fact', 'edit');
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

  editClicked(fact: FactCreate) {
    this.onEdit.emit(fact);
  }
}
