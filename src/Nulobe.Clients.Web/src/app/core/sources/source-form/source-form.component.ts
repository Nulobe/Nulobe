import { Component, OnInit, OnDestroy, Input, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { FormBuilder, FormArray, FormGroup, FormControl } from '@angular/forms';
import { MdInputDirective } from '@angular/material';
import { SourceType, ApaSourceType } from '../../api';

import { Source } from '../source';
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
  pagesFormGroup: FormGroup;

  @ViewChildren(MdInputDirective) inputs: QueryList<MdInputDirective>;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    let { fb, source } = this;

    this.sourceAuthorsFormArray = fb.array((source.authors || []).map(a => this.createAuthorControl(a)));
    
    this.newAuthorFormControl = fb.control('');
    
    this.pagesFormGroup = fb.group({
      pageStart: fb.control(source.pages ? source.pages.pageStart : null),
      pageEnd: fb.control(source.pages ? source.pages.pageEnd : null)
    });

    this.sourceFormGroup = fb.group({
      type: fb.control(source.type),
      apaType: fb.control(source.apaType),
      authors: this.sourceAuthorsFormArray,
      newAuthor: this.newAuthorFormControl,
      organisation: fb.control(source.organisation),
      title: fb.control(source.title),
      pages: this.pagesFormGroup,
      doi: fb.control(source.doi),
      citation: fb.control(source.citation),
      notesMarkdown: fb.control(source.notesMarkdown),
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
    return SourcePropertyHelper.hasProperty(type, propName);
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

      let newAuthorInput = this.inputs.filter(input => input.id === "newAuthor")[0];
      newAuthorInput.focus();
    }
  }
}
