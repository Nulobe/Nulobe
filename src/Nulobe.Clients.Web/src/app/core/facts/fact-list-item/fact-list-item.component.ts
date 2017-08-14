import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { FactCreate } from '../../api/api.swagger';

@Component({
  selector: 'app-fact-list-item',
  templateUrl: './fact-list-item.component.html',
  styleUrls: ['./fact-list-item.component.scss']
})
export class FactListItemComponent implements OnInit {
  @Input() fact: FactCreate;
  @Output() onTagClick = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  tagClicked(tag: string) {
    this.onTagClick.emit(tag);
  }

}