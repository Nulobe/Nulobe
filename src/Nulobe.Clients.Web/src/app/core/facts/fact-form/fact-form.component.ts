import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { FactCreate, Source } from '../../api';

interface FactFormValue {
  title: string;
  definition: string;
  notes: string;
  indexedSources: Source[];
}

@Component({
  selector: 'app-fact-form',
  templateUrl: './fact-form.component.html',
  styleUrls: ['./fact-form.component.scss']
})
export class FactFormComponent implements OnInit {
  @Input() fact: FactCreate;
  @Output() factChanges = new EventEmitter<FactCreate>();
  @Output() factValidChanges = new EventEmitter<boolean>();

  private form: FormGroup;
  private tags: string[] = [];
  private sourceCount$: Observable<number>;
  private lastFormValid: boolean;
  private lastValid: boolean;

  constructor(
    private fb: FormBuilder) { }

  ngOnInit() {
    this.fact = this.fact || <FactCreate>{
      title: '',
      definition: '',
      notes: '',
      sources: [],
      tags: []
    };

    let { fb } = this;

    this.form = fb.group({
      title: fb.control(this.fact.title, Validators.required),
      definition: fb.control(this.fact.definition, Validators.required),
      notes: fb.control(this.fact.notes),
      indexedSources: fb.array(this.fact.sources.map(s => this.createIndexedSource(s)))
    });

    this.sourceCount$ = this.form.valueChanges.map((formValue: FactFormValue) => {
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

      return Math.min(maxSourceIndex, 10);
    });

    this.sourceCount$.subscribe(sourceCount => {
      let indexedSources = this.form.get('indexedSources') as FormArray;
      let existingSourceCount = indexedSources.controls.length;
      
      let sourceCountDiff = sourceCount - existingSourceCount;
      if (sourceCountDiff > 0) {
        for (let i = 0; i < sourceCountDiff; i++) {
          indexedSources.push(this.createIndexedSource());
        }
      } else if (sourceCountDiff < 0) {
        let lastUnindexedSource = (<any[]>indexedSources.value)
          .filter((v, i) => i >= sourceCount)
          .reduce((prev, curr, i) => {
            let { url, description } = curr;
            return url || description ? i : prev;
          }, -1);
        
        for (let i = sourceCount + 1 + lastUnindexedSource; i < existingSourceCount; i++) {
          indexedSources.removeAt(i);
        }
      }
    });

    this.form.valueChanges.subscribe(() => {
      this.triggerFactValidChanges();
      this.triggerFactChanges();
    });
  }

  private updateTags(tags: string[]) {
    this.tags = tags;
    this.triggerFactChanges();
  }

  private triggerFactChanges() {
    let formValue: FactFormValue = this.form.value;
    this.fact = {
      title: formValue.title || '',
      definition: formValue.definition || '',
      sources: formValue.indexedSources,
      notes: formValue.notes,
      tags: this.tags
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
    return this.tags.length && this.form.valid;
  }

  private createIndexedSource(source?: Source): FormGroup {
    let { fb } = this;
    return fb.group({
      url: fb.control(source ? source.url : ''),
      description: fb.control(source ? source.description : '')
    });
  }

}