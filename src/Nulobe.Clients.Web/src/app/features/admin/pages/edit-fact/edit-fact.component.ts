import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

import { FactApiClient, Fact, SwaggerException } from '../../../../core/api';

import { FactPreviewDialogService } from '../fact-preview-dialog/fact-preview-dialog.service';

@Component({
  selector: 'app-edit-fact',
  templateUrl: './edit-fact.component.html',
  styleUrls: ['./edit-fact.component.scss']
})
export class EditFactComponent implements OnInit {

  private factId: string;
  private fact: Fact;
  private valid: boolean;
  private error: any = null;
  private hasError: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private factApiClient: FactApiClient,
    private factPreviewDialogService: FactPreviewDialogService
  ) { }

  ngOnInit() {
    this.route.params.map(p => p.factId).subscribe(factId => {
      this.factId = factId;
    });

    this.route.data.map(d => d.fact).subscribe(f => {
      this.fact = f;
    });
  }

  previewFact() {
    this.factPreviewDialogService.open(this.fact);
  }

  updateFact() {
    this.error = null;
    this.hasError = false;

    if (window.confirm('Are you sure you want to update this fact?')) {
      this.factApiClient.update(this.factId, this.fact)
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
