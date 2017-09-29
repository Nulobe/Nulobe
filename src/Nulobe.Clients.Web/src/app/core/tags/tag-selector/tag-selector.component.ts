import { Component, OnInit, HostListener, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/debounceTime';

import { TagApiClient, Tag } from '../../api/api.swagger';

interface TagModel {
  display: string;
  value: string;
}

@Component({
  selector: 'core-tag-selector',
  templateUrl: './tag-selector.component.html',
  styleUrls: ['./tag-selector.component.scss']
})
export class TagSelectorComponent implements OnInit {
  @Input() tags: string[];
  @Input() placeholder: string;
  @Input() secondaryPlaceholder: string;
  @Output() onTagsUpdated = new EventEmitter<string[]>();
  @Output() onInputFocusUpdated = new EventEmitter<boolean>();
  @Output() onSubmit = new EventEmitter();

  tagInput_tags: TagModel[] = [];
  tagInput_isFocused = false;
  tagInput_currentText = '';
  private tagInput_lastApiCallText: string = null;
  private tagInput_lastApiCall: Promise<Tag[]>;

  constructor(
    private tagApiClient: TagApiClient,
    private elementRef: ElementRef
  ) { }

  ngOnInit() {
    this.tags = this.tags || [];
    this.tagInput_tags = this.tags.map(t => this.createTagModel(t));
    this.placeholder = this.placeholder || "+ Tag";
    this.secondaryPlaceholder  = this.secondaryPlaceholder || "Enter a new tag";

    // Ensure production API warms up as soon as possible.
    this.tagInput_lastApiCallText = '';
    this.tagInput_lastApiCall = this.getApiSuggestions('');
  }

  focus() {
    let input = this.elementRef.nativeElement.querySelector('input');
    input.focus();
    this.tagInput_focusUpdated(true);
  }

  tagInput_getSuggestions = (text: string): Observable<Tag[]> => {
    if (text === '') {
      return Observable.of([]);
    } else {
      let apiSuggestions: Promise<Tag[]> = null;
      if (this.tagInput_lastApiCallText === '' || (this.tagInput_lastApiCallText && text.includes(this.tagInput_lastApiCallText))) {
        // Retrieve a subset of the last API results
        let normalizeText = text.toLowerCase();
        apiSuggestions = this.tagInput_lastApiCall.then(tags =>
          tags.filter(t => t.text.toLowerCase().includes(normalizeText)));

      } else {
        this.tagInput_lastApiCallText = text;
        apiSuggestions = this.getApiSuggestions(text);
        this.tagInput_lastApiCall = apiSuggestions;
      }

      return Observable.fromPromise(apiSuggestions.then(r => {
        r.sort((a, b) => b.usageCount - a.usageCount);
        return r.filter(tag => {
          let existing = this.tags.find(tagString => tagString.toLowerCase() === tag.text.toLowerCase());
          return !existing;
        })
        .filter((tag, index) => index < 3);
      }));
        
    }
  }

  tagInput_matchingFunc = (): boolean => true;

  tagInput_addHash = (text: string | Tag): Observable<TagModel> => {
    if (typeof(text) === 'object') {
      text = text.text;
    }
    return Observable.of(this.createTagModel(text));
  }

  tagInput_updated = () => {
    this.tags = this.tagInput_tags.map(t => t.value);
    this.onTagsUpdated.emit(this.tags);
  }

  tagInput_focusUpdated = (value) => {
    let currentValue = this.tagInput_isFocused;
    if (currentValue !== value) {
      this.tagInput_isFocused = value;
      this.onInputFocusUpdated.emit(value);
    }
  }

  @HostListener('document:keypress', ['$event'])
  host_handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode === 13 && this.tagInput_isFocused && !this.tagInput_currentText && this.tags.length) {
      this.onSubmit.emit();
    }
  }

  createTagModel(text: string): TagModel {
    return {
      display: `${text}`,
      value: `${text}`
    };
  }

  getApiSuggestions(text: string) {
    return this.tagApiClient
      .list(text, "text,usagecount", "usagecount")
      .toPromise();
  }
}
