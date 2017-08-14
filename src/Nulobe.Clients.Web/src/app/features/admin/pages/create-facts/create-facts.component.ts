import { Component, OnInit } from '@angular/core';
import { MdDialog } from '@angular/material';
import { Observable } from 'rxjs';

import { FactApiClient, FactCreate, SwaggerException } from '../../../../core/api';

import { FactPreviewDialogComponent } from '../fact-preview-dialog/fact-preview-dialog.component';

@Component({
  selector: 'app-create-facts',
  templateUrl: './create-facts.component.html',
  styleUrls: ['./create-facts.component.scss']
})
export class CreateFactsComponent implements OnInit {

  private fact: FactCreate;
  private valid: boolean;
  private error: any = null;
  private hasError: boolean = false;

  constructor(
    private factApiClient: FactApiClient,
    private mdDialog: MdDialog
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
    let dialogRef = this.mdDialog.open(FactPreviewDialogComponent);
    dialogRef.componentInstance.fact = this.fact;
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
