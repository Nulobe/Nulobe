import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { FactApiClient, FactCreate, SwaggerException } from '../../../../core/api';

import { FactPreviewDialogComponent } from '../fact-preview-dialog/fact-preview-dialog.component';
import { FactPreviewDialogService } from '../fact-preview-dialog/fact-preview-dialog.service';

@Component({
  selector: 'app-create-fact',
  templateUrl: './create-fact.component.html',
  styleUrls: ['./create-fact.component.scss']
})
export class CreateFactComponent implements OnInit {

  private fact: FactCreate;
  private valid: boolean;
  private error: any = null;
  private hasError: boolean = false;

  constructor(
    private factApiClient: FactApiClient,
    private factPreviewDialogService: FactPreviewDialogService
  ) { }

  ngOnInit() {
    this.fact = {
      title: '',
      definition: '',
      sources: [],
      tags: []
    };
    this.valid = false;
  }

  previewFact() {
    this.factPreviewDialogService.open(this.fact);
  }

  publishFact() {
    this.error = null;
    this.hasError = false;

    if (window.confirm('Are you sure you want to publish this fact?')) {
      this.factApiClient.create(this.fact)
        .catch((err: SwaggerException) => {
          this.hasError = true;
          if (err.response) {
            this.error = JSON.parse(err.response);
          }
          return Observable.empty();
        })
        .subscribe(() => {
          alert('success!');
        });
    }
  }

}
