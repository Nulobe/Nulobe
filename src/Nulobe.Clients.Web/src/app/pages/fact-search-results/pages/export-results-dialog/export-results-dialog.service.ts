import { Injectable } from '@angular/core';
import { MdDialog } from '@angular/material';

import { Fact } from '../../../../core/api';

import { ExportResultsDialogComponent } from './export-results-dialog.component';

@Injectable()
export class ExportResultsDialogService {

  constructor(
    private mdDialog: MdDialog
  ) { }

  open(tags: string[], facts: Fact[]) {
    let dialogRef = this.mdDialog.open(ExportResultsDialogComponent);
    dialogRef.componentInstance.tags = tags;
    dialogRef.componentInstance.facts = facts;
    dialogRef.componentInstance.onSuccess.first().subscribe(() => {
      dialogRef.close();
    });
  }

}
