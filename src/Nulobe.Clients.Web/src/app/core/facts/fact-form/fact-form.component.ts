import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter, ChangeDetectorRef  } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { Observable, BehaviorSubject } from 'rxjs';

import { NULOBE_ENV_SETTINGS } from '../../../app.settings';
import { FactCreate } from '../../api';
import { Source, SourceType, SourcePropertyHelper } from '../../sources';

interface FactFormValue {
  title: string;
  definition: string;
  notesMarkdown: string;
  sources: Source[];
  country: string;
}

@Component({
  selector: 'core-fact-form',
  templateUrl: './fact-form.component.html',
  styleUrls: ['./fact-form.component.scss']
})
export class FactFormComponent implements OnInit, AfterViewInit {
  @Input() fact: FactCreate;
  @Output() factChanges = new EventEmitter<FactCreate>();
  @Output() factValidChanges = new EventEmitter<boolean>();

  form: FormGroup;
  countries = NULOBE_ENV_SETTINGS.countries;
  sourceCount$: Observable<number>;

  private lastFormValid: boolean;
  private lastValid: boolean;
  private sourceCount = new BehaviorSubject<number>(0);  

  constructor(
    private fb: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit() {
    this.fact = this.fact || <FactCreate>{
      title: '',
      definition: '',
      notesMarkdown: '',
      sources: [],
      tags: []
    };

    let { fb } = this;

    this.form = fb.group({
      title: fb.control(this.fact.title, Validators.required),
      definition: fb.control(this.fact.definition, Validators.required),
      notesMarkdown: fb.control(this.fact.notesMarkdown),
      country: fb.control(this.fact.country)
    });

    this.sourceCount$ = this.sourceCount.asObservable();
  }

  ngAfterViewInit(): void {
    this.updateSourceCount(this.form.value);
    this.form.valueChanges.subscribe((formValue: FactFormValue) => {
      this.updateSourceCount(formValue);
    });

    this.form.valueChanges.subscribe(() => {
      this.triggerFactValidChanges();
      this.triggerFactChanges();
    });

    this.changeDetectorRef.detectChanges();
  }

  updateTags(tags: string[]) {
    this.fact.tags = tags;
    this.triggerFactChanges();
  }

  private triggerFactChanges() {
    let formValue: FactFormValue = this.form.value;
    this.fact = {
      title: formValue.title || '',
      definition: formValue.definition || '',
      sources: formValue.sources.map(s => this.pruneSource(s)),
      notesMarkdown: formValue.notesMarkdown,
      tags: this.fact.tags,
      country: formValue.country
    };
    this.factChanges.emit(this.fact);
  }

  private triggerFactValidChanges() {
    let valid = this.isValid();
    if (valid !== this.lastValid) {
      this.factValidChanges.emit(valid);
      this.lastValid = valid;
    }
  }

  private isValid() {
    return this.fact.tags.length && this.form.valid;
  }

  private updateSourceCount(formValue: FactFormValue) {
    let sourceReferenceRegex = /\[(\d+)\]/g;
    let definition = formValue.definition;

    let matches = [];
    let match = null;
    while ((match = sourceReferenceRegex.exec(definition)) != null) {
      matches.push(parseInt(match[1], 10));
    }

    let maxSourceIndex =  matches
      .filter(i => i > 0)
      .reduce((prev, curr) => Math.max(prev, curr), 0);

    this.sourceCount.next(Math.min(maxSourceIndex, 10));
  }

  private pruneSource(source: Source): Source {
    let result: Source = { type: source.type };
    
    let properties = SourcePropertyHelper.getProperties(source.type);
    properties.forEach(p => {
      result[p] = source[p];
    });

    return result;
  }

}
