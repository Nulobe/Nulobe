import { Component, OnInit, HostListener, Input, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/debounceTime';

import { TagApiClient, Tag } from '../../api/api.swagger';

interface TagModel {
  display: string;
  value: string;
}

@Component({
  selector: 'app-tag-selector',
  templateUrl: './tag-selector.component.html',
  styleUrls: ['./tag-selector.component.scss']
})
export class TagSelectorComponent implements OnInit {
  @Input() secondaryPlaceholder: string;
  @Output() onTagsUpdated = new EventEmitter<string[]>();
  @Output() onSubmit = new EventEmitter();

  private tags: TagModel[] = [];
  private tagInput_isFocused = false;
  private tagInput_currentText = '';
  private tagInput_lastApiCallText: string = null;
  private tagInput_lastApiCall: Promise<Tag[]>;

  constructor(
    private tagApiClient: TagApiClient
  ) { }

  ngOnInit() {
    if (!this.secondaryPlaceholder) {
      this.secondaryPlaceholder = "Enter a new tag";
    }
  }

  tagInput_getSuggestions = (text: string): Observable<string[]> => {
    if (text === '') {
      return Observable.of([]);
    } else {
      let apiSuggestions: Promise<Tag[]> = null;
      if (this.tagInput_lastApiCallText && text.includes(this.tagInput_lastApiCallText)) {
        // Retrieve a subset of the last API results
        let normalizeText = text.toLowerCase();
        apiSuggestions = this.tagInput_lastApiCall.then(tags =>
          tags.filter(t => t.text.toLowerCase().includes(normalizeText)));

      } else {
        this.tagInput_lastApiCallText = text;

        apiSuggestions = this.tagApiClient
          .list(text, undefined, undefined)
          .toPromise();

        this.tagInput_lastApiCall = apiSuggestions;
      }

      return Observable.fromPromise(apiSuggestions.then(r => r
        .filter(x => {
          let existing = this.tags.find(y => y.display.substring(1).toLowerCase() === x.text.toLowerCase());
          return !existing;
        })
        .map(t => t.text)))
    }
  }

  tagInput_addHash = (text: string | TagModel): Observable<TagModel> => {
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
