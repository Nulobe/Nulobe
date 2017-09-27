import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { FactApiClient, Fact, SwaggerException } from '../../../../core/api';

import { FactPreviewDialogComponent } from '../fact-preview-dialog/fact-preview-dialog.component';
import { FactPreviewDialogService } from '../fact-preview-dialog/fact-preview-dialog.service';

@Component({
  selector: 'admin-create-fact',
  templateUrl: './create-fact.component.html',
  styleUrls: ['./create-fact.component.scss']
})
export class CreateFactComponent implements OnInit {

  fact: Fact;
  valid: boolean;
  error: any = null;
  hasError: boolean = false;

  constructor(
    private factApiClient: FactApiClient,
    private factPreviewDialogService: FactPreviewDialogService,
    private router: Router
  ) { }

  ngOnInit() {
    this.fact = {
      title: '',
      definitionMarkdown: '',
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
      this.factApiClient.create(this.fact, false)
        .catch((err: SwaggerException) => {
          this.hasError = true;
          if (err.response) {
            this.error = JSON.parse(err.response);
          }
          return Observable.empty();
        })
        .subscribe(() => {
          this.router.navigate(['/LOBE/admin']);
        });
    }
  }

}
