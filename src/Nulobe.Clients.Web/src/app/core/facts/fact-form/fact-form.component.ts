import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Fact, Tag } from '../../api';

@Component({
  selector: 'app-fact-form',
  templateUrl: './fact-form.component.html',
  styleUrls: ['./fact-form.component.scss']
})
export class FactFormComponent implements OnInit, OnChanges {
  @Input() fact: Fact;
  @Output() factChanges = new EventEmitter<Fact>();
  @Output() factValidChanges = new EventEmitter<boolean>();

  private form: FormGroup;
  private tags: Tag[] = [];
  private sourceCount$: Observable<number>;
  private lastFormValid: boolean;
  private lastValid: boolean;

  constructor(
    private fb: FormBuilder) { }

  ngOnInit() {
    let { fb } = this;

    this.form = fb.group({
      title: fb.control('', Validators.required),
      description: fb.control('', Validators.required),
      notes: fb.control(''),
      indexedSources: fb.array([])
    });

    this.sourceCount$ = this.form.valueChanges.map(v => {
      let sourceReferenceRegex = /\[(\d+)\]/g;
      let description = v['description'];

      let matches = [];
      let match = null;
      while ((match = sourceReferenceRegex.exec(description)) != null) {
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

    this.fact = this.fact || {};

    this.form.valueChanges.subscribe(f => {
      this.triggerFactValidChanges();
      this.triggerFactChanges();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    debugger;
  }

  private triggerFactChanges() {
    if (this.isValid()) {
      let formValue = this.form.value;
      this.fact = {
        title: formValue.title,
        tags: this.tags.map(t => t.text),
        definition: formValue.definition,
        sources: formValue.indexedSources,
        //notes: formValue.notes
      };
      this.factChanges.emit(this.fact);
    }
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

  private createIndexedSource(): FormGroup {
    let { fb } = this;
    return fb.group({
      url: fb.control(''),
      description: fb.control('')
    });
  }

}
