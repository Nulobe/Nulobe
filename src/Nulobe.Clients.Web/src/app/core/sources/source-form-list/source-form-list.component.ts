import { Component, OnInit, OnChanges, SimpleChanges, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';

import { SourceType, ApaSourceType } from '../../api';

import { Source } from '../source';

let trackByIdCounter = 0;

@Component({
  selector: 'core-source-form-list',
  templateUrl: './source-form-list.component.html',
  styleUrls: ['./source-form-list.component.scss']
})
export class SourceFormListComponent implements OnInit, OnChanges {
  @Input() parentForm: FormGroup;
  @Input() sources: any[];
  @Input() sourceCount: number;

  localSources: Source[];
  sourcesFormArray: FormArray;
  sourceIndexRange: number[];

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.localSources = this.sources.map(s => Object.assign({}, s, { trackById: ++trackByIdCounter }));
    this.sourcesFormArray = this.fb.array([]);
    this.parentForm.addControl('sources', this.sourcesFormArray);

    this.sourcesFormArray.valueChanges.subscribe(v => {
      this.removeUnusedSources();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes.sourceCount) {
      return;
    }
    
    let { sourcesFormArray, localSources, sourceCount } = this;

    if (this.localSources) {
      let existingSourceCount = localSources.length;
      let sourceCountDiff = sourceCount - existingSourceCount;
  
      if (sourceCountDiff > 0) {
        for (let i = 0; i < sourceCountDiff; i++) {
          localSources.push({
            type: SourceType.CitationNeeded,
            apaType: ApaSourceType.JournalArticle,
            trackById: ++trackByIdCounter,
            authors: []
          });
        }
      } else if (sourceCountDiff < 0) {
        this.removeUnusedSources();
      }
    }
  }

  trackSource(source: Source): number {
    return source.trackById;
  }

  removeUnusedSources() {
    let { sourcesFormArray, localSources, sourceCount } = this;

    let lastOrphanedSource = (<Source[]>sourcesFormArray.value)
      .filter((v, i) => i >= sourceCount)
      .reduce((prev, curr, i) => {
        let { url, description } = curr;
        return url || description ? i : prev;
      }, -1);

    let removeFromIndex = sourceCount + lastOrphanedSource + 1;
    localSources.splice(removeFromIndex);
  }
}
