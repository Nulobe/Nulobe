import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { MdDialog } from '@angular/material';

import { Fact, FactApiClient, API_BASE_URL } from '../../../../core/api';

import { FactPreviewDialogComponent } from './fact-preview-dialog.component';

@Injectable()
export class FactPreviewDialogService {

  constructor(
    private mdDialog: MdDialog,
    private factApiClient: FactApiClient,
    private http: Http,
    @Inject(API_BASE_URL) private baseUrl: string
  ) { }

  open(fact: Fact) {
    this.http.post(this.baseUrl + '/facts?dryRun=true', fact)
      .map(r => r.json() as Fact)
      .subscribe(f => {
        let dialogRef = this.mdDialog.open(FactPreviewDialogComponent);
        dialogRef.componentInstance.fact = f;
      });
  }

}
