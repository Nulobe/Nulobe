import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs';

import { SourceJournal } from '../source';

@Component({
  selector: 'core-source-journal',
  templateUrl: './source-journal.component.html',
  styleUrls: ['./source-journal.component.scss']
})
export class SourceJournalComponent implements OnInit, OnDestroy {

  @Input() parentFormGroup: FormGroup;
  @Input() sourceJournal: SourceJournal;

  journalFormGroup: FormGroup;
  titleFormControl: FormControl;
  volumeFormControl: FormControl;
  issueFormControl: FormControl;
  
  constructor(
    private fb: FormBuilder
  ) { }
  
  ngOnInit() {
    let { fb, sourceJournal } = this;

    this.titleFormControl = fb.control(sourceJournal ? sourceJournal.title : null);
    this.volumeFormControl = fb.control(sourceJournal ? sourceJournal.volume : null);
    this.issueFormControl = fb.control(sourceJournal ? sourceJournal.issue : null);

    let beforeDisabledVolumeValue = null;
    let beforeDisabledIssueValue = null;

    this.titleFormControl.valueChanges.subscribe(v => {
      if (v) {
        this.volumeFormControl.enable();
      } else {
        this.volumeFormControl.disable();
      }
    });

    this.volumeFormControl.valueChanges.subscribe(v => {
      if (v) {
        this.issueFormControl.enable();
      } else {
        this.issueFormControl.disable();
      }
    });

    this.journalFormGroup = fb.group({
      title: this.titleFormControl,
      volume: this.volumeFormControl,
      issue: this.issueFormControl
    });

    this.parentFormGroup.addControl('journal', this.journalFormGroup);
  }
  
  ngOnDestroy() {
    this.parentFormGroup.removeControl('journal');
  }
}
