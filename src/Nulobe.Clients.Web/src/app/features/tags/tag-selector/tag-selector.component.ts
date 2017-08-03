import { Component, OnInit, HostListener, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/debounceTime';

import { TagApiClient } from '../../api/api.swagger';

interface Tag {
  display: string;
  value: string;
}

@Component({
  selector: 'app-tag-selector',
  templateUrl: './tag-selector.component.html',
  styleUrls: ['./tag-selector.component.scss']
})
export class TagSelectorComponent implements OnInit {
  @Output() onTagsUpdated = new EventEmitter<string[]>();
  @Output() onSubmit = new EventEmitter();

  private tags: Tag[] = [];
  private tagInput_isFocused = false;
  private tagInput_currentText = '';

  constructor(
    private tagApiClient: TagApiClient
  ) { }

  ngOnInit() {
  }

  tagInput_getSuggestions = (text: string): Observable<string[]> =>
    this.tagApiClient
      .list(text, undefined, undefined)
      .map(r => r.map(t => t.text));

  tagInput_addHash = (text: string | Tag): Observable<Tag> => {
    if (typeof(text) === 'object') {
      text = text.display;
    }

    let item = {
      display: `#${text}`,
      value: `#${text}`
    };

    return Observable.of(item);
  }

  tagInput_updated = () => {
    let tags = this.tags.map(t => t.value.substring(1, t.value.length));
    this.onTagsUpdated.emit(tags);
  }

  @HostListener('document:keypress', ['$event'])
  host_handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode === 13 && this.tagInput_isFocused && !this.tagInput_currentText) {
      this.onSubmit.emit();
    }
  }
}
