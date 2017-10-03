import { Component, OnInit, OnDestroy, Input, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { FormBuilder, FormArray, FormGroup, FormControl, ValidatorFn } from '@angular/forms';
import { MdInputDirective } from '@angular/material';
import { Observable } from 'rxjs';

import { Source } from '../source';
import { SourceType, ApaSourceType } from '../source-type';
import { SourcePropertyHelper } from '../helpers/source-property.helper';

const MINIMUM_YEAR = 1900;
const CURRENT_YEAR = new Date().getFullYear();
const NUMBER_OF_YEARS = CURRENT_YEAR - MINIMUM_YEAR + 1;
const NUMBER_OF_MONTHS = 12;

@Component({
  selector: 'core-source-form',
  templateUrl: './source-form.component.html',
  styleUrls: ['./source-form.component.scss']
})
export class SourceFormComponent implements OnInit, OnDestroy {
  
  @Input() parentFormArray: FormArray;
  @Input() source: Source;
  @Input() sourceIndex: number;

  sourceFormGroup: FormGroup;
  sourceAuthorsFormArray: FormArray;
  newAuthorFormControl: FormControl;
  datePartsFormGroup: FormGroup;
  yearFormControl: FormControl;
  monthFormControl: FormControl;
  dayFormControl: FormControl;

  years = Array.from(new Array(NUMBER_OF_YEARS), (x, i) => CURRENT_YEAR - i);
  months = Array.from(new Array(NUMBER_OF_MONTHS), (x, i) => {
    let d = new Date();
    d.setMonth(i);
    return d.toLocaleDateString(navigator.language || 'en-us', { month: 'long' });
  });
  days$: Observable<number[]>;

  @ViewChildren(MdInputDirective) newAuthor: QueryList<MdInputDirective>;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    let { fb, source } = this;
    let { date } = source;

    this.yearFormControl = fb.control(
      date ? date.year : null,
      c => {
        if (c.value < MINIMUM_YEAR || c.value > CURRENT_YEAR) {
          return { 'year': 'Year is invalid' };
        }
        return null;
      });

    this.monthFormControl = fb.control(
      date ? date.month : null,
      c => {
        if (c.value < 1 || c.value > 12) {
          return { 'month': 'Month is invalid' };
        }
      });
    this.dayFormControl = fb.control(date ? date.day : null);

    this.yearFormControl.valueChanges.subscribe(v => {
      this.monthFormControl.setValue(null);
      if (this.yearFormControl.valid) {
        this.monthFormControl.enable()
      } else {
        this.monthFormControl.disable();
      }
    });

    this.monthFormControl.valueChanges.subscribe(v => {
      this.dayFormControl.setValue(null);
      if (this.monthFormControl.valid) {
        this.dayFormControl.enable();
      } else {
        this.dayFormControl.disable();
      }
    });

    this.days$ = Observable
      .combineLatest([this.yearFormControl.valueChanges, this.monthFormControl.valueChanges])
      .map(([year, month]) => {
        if (month) {
          let numberOfDays = new Date(year, month, 0).getDate();
          return Array.from(new Array(numberOfDays), (x, i) => i + 1);
        } else {
          return [];
        }
      });

    this.datePartsFormGroup = fb.group({
      year: this.yearFormControl,
      month: this.monthFormControl,
      day: this.dayFormControl
    });

    this.sourceAuthorsFormArray = fb.array(source.authors.map(a => this.createAuthorControl(a)));
    this.newAuthorFormControl = fb.control('');
    this.sourceFormGroup = fb.group({
      type: fb.control(source.type),
      apaType: fb.control(source.apaType),
      title: fb.control(source.title),
      authors: this.sourceAuthorsFormArray,
      newAuthor: this.newAuthorFormControl,
      url: fb.control(source.url),
      description: fb.control(source.description),
      factId: fb.control(source.factId),
      date: this.datePartsFormGroup
    });

    this.parentFormArray.push(this.sourceFormGroup);
  }

  ngOnDestroy() {
    this.parentFormArray.removeAt(this.sourceIndex);
  }

  hasProperty(propName: string) {
    let type: SourceType = this.sourceFormGroup.get('type').value;
    let apaType: ApaSourceType = this.sourceFormGroup.get('apaType').value;
    return SourcePropertyHelper.hasProperty(type, apaType, propName);
  }

  createAuthorControl(author: string): FormControl {
    return this.fb.control(author);
  }

  onAuthorBlur(authorIndex) {
    let authorControl = this.sourceAuthorsFormArray.get(authorIndex.toString());
    if (!authorControl.value) {
      this.sourceAuthorsFormArray.removeAt(authorIndex);
    }
  }

  onNewAuthorBlur() {
    let { value } = this.newAuthorFormControl;
    if (value) {
      this.sourceAuthorsFormArray.push(this.createAuthorControl(value));
      this.newAuthorFormControl.setValue('');
      this.newAuthor.last.focus();
    }
  }
}
