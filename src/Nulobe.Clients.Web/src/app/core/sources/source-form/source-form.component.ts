import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder, FormArray, FormGroup } from '@angular/forms';

import { Source } from '../source';
import { SourceType } from '../source-type';
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

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.source = this.source || <Source>{
      type: SourceType.CitationNeeded,
      url: '',
      description: ''
    };

    let { fb, source } = this;

    this.sourceFormGroup = fb.group({
      type: fb.control(source.type),
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
}
