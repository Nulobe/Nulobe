import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs';

import { SourceDate } from '../source';

const MINIMUM_YEAR = 1900;
const CURRENT_YEAR = new Date().getFullYear();
const NUMBER_OF_YEARS = CURRENT_YEAR - MINIMUM_YEAR + 1;
const NUMBER_OF_MONTHS = 12;

@Component({
  selector: 'core-source-date',
  templateUrl: './source-date.component.html',
  styleUrls: ['./source-date.component.scss']
})
export class SourceDateComponent implements OnInit, OnDestroy {

  @Input() parentFormGroup: FormGroup;
  @Input() sourceDate: SourceDate;

  dateFormGroup: FormGroup;
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

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    let { fb, sourceDate } = this;

    const isYearInRange = (x) => x >= MINIMUM_YEAR && x <= CURRENT_YEAR;
    const isMonthInRange = (x) => x >= 1 && x <= 12;

    this.yearFormControl = fb.control(sourceDate ? sourceDate.year : null);
    this.monthFormControl = fb.control(sourceDate ? sourceDate.month : null);
    this.dayFormControl = fb.control(sourceDate ? sourceDate.day : null);

    this.yearFormControl.valueChanges.subscribe(v => {
      this.monthFormControl.setValue(null);
      if (isYearInRange(v)) {
        this.monthFormControl.enable()
      } else {
        this.monthFormControl.disable();
      }
    });

    this.monthFormControl.valueChanges.subscribe(v => {
      this.dayFormControl.setValue(null);
      if (isMonthInRange(v)) {
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

    this.dateFormGroup = fb.group({
      year: this.yearFormControl,
      month: this.monthFormControl,
      day: this.dayFormControl
    });

    this.parentFormGroup.addControl('date', this.dateFormGroup);
  }

  ngOnDestroy(): void {
    this.parentFormGroup.removeControl('date');
  }
}
