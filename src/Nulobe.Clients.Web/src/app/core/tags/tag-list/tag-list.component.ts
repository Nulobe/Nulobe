import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.scss']
})
export class TagListComponent implements OnInit {
  @Input() tags: string[];
  @Output() onTagClick = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  tagClicked(tag: string) {
    this.onTagClick.emit(tag);
  }
}
