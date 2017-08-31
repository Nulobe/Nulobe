import { Component, OnInit, HostListener, Input, Output, EventEmitter, ElementRef } from '@angular/core';
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
  @Input() tags: string[];
  @Input() secondaryPlaceholder: string;
  @Output() onTagsUpdated = new EventEmitter<string[]>();
  @Output() onInputFocusUpdated = new EventEmitter<boolean>();
  @Output() onSubmit = new EventEmitter();

  private tagInput_tags: TagModel[] = [];
  private tagInput_isFocused = false;
  private tagInput_currentText = '';
  private tagInput_lastApiCallText: string = null;
  private tagInput_lastApiCall: Promise<Tag[]>;

  constructor(
    private tagApiClient: TagApiClient,
    private elementRef: ElementRef
  ) { }

  ngOnInit() {
    this.tags = this.tags || [];
    this.tagInput_tags = this.tags.map(t => this.createTagModel(t));
    this.secondaryPlaceholder  = this.secondaryPlaceholder || "Enter a new tag";
  }

  focus() {
    let input = this.elementRef.nativeElement.querySelector('input');
    input.focus();
    this.tagInput_focusUpdated(true);
  }

  private tagInput_getSuggestions = (text: string): Observable<string[]> => {
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
        .filter(tag => {
          let existing = this.tags.find(tagString => tagString.substring(1).toLowerCase() === tag.text.toLowerCase());
          return !existing;
        })
        .map(t => t.text)))
    }
  }

  private tagInput_addHash = (text: string | TagModel): Observable<TagModel> => {
    if (typeof(text) === 'object') {
      text = text.display;
    }
    return Observable.of(this.createTagModel(text));
  }

  private tagInput_updated = () => {
    this.tags = this.tagInput_tags.map(t => t.value.substring(1, t.value.length));
    this.onTagsUpdated.emit(this.tags);
  }

  private tagInput_focusUpdated = (value) => {
    let currentValue = this.tagInput_isFocused;
    if (currentValue !== value) {
      this.tagInput_isFocused = value;
      this.onInputFocusUpdated.emit(value);
    }
  }

  @HostListener('document:keypress', ['$event'])
  private host_handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode === 13 && this.tagInput_isFocused && !this.tagInput_currentText) {
      this.onSubmit.emit();
    }
  }

  private createTagModel(text: string): TagModel {
    return {
      display: `#${text}`,
      value: `#${text}`
    };
  }
}
