import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { FactApiClient, Fact, SwaggerException } from '../../../../core/api';

import { FactPreviewDialogService } from '../fact-preview-dialog/fact-preview-dialog.service';

@Component({
  selector: 'admin-edit-fact',
  templateUrl: './edit-fact.component.html',
  styleUrls: ['./edit-fact.component.scss']
})
export class EditFactComponent implements OnInit {

  fact: Fact;

  private masterFact: Fact;
  private factId: string;
  private valid: boolean = true;
  private error: any = null;
  private hasError: boolean = false;

  private deleteForm: FormGroup;
  private deleteError: any = null;
  private hasDeleteError: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private factApiClient: FactApiClient,
    private factPreviewDialogService: FactPreviewDialogService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.route.params.map(p => p.factId).subscribe(factId => {
      this.factId = factId;
    });

    this.route.data.map(d => d.fact).subscribe(f => {
      this.masterFact = f;
      this.fact = f;
    });

    this.deleteForm = this.formBuilder.group({
      title: this.formBuilder.control(''),
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
          this.router.navigate(['/LOBE/admin']);
        });
    }
  }

  deleteFact() {
    this.deleteError = null;
    this.hasDeleteError = false;

    if (this.deleteFactConfirmed()) {
      this.factApiClient.delete(this.factId)
        .catch((err: SwaggerException) => {
          this.hasDeleteError = true;
          if (err.response) {
            this.deleteError = JSON.parse(err.response);
          }
          return Observable.empty();
        })
        .subscribe(() => {
          this.router.navigate(['/LOBE/admin']);
        });
    }
  }

  deleteFactConfirmed(): boolean {
    return this.deleteForm.value.title == this.masterFact.title;
  }
}
