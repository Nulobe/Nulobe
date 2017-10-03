import { Component, OnInit, OnDestroy, Input, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { FormBuilder, FormArray, FormGroup, FormControl } from '@angular/forms';
import { MdInputDirective } from '@angular/material';

import { Source } from '../source';
import { SourceType, ApaSourceType } from '../source-type';
import { SourcePropertyHelper } from '../helpers/source-property.helper';

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

  @ViewChildren(MdInputDirective) newAuthor: QueryList<MdInputDirective>;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    let { fb, source } = this;

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
      factId: fb.control(source.factId)
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
