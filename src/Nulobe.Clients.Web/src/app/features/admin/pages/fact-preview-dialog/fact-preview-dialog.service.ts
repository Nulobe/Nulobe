import { Injectable } from '@angular/core';
import { MdDialog } from '@angular/material';

import { Fact } from '../../../../core/api';

import { FactPreviewDialogComponent } from './fact-preview-dialog.component';

@Injectable()
export class FactPreviewDialogService {

  constructor(
    private mdDialog: MdDialog
  ) { }

  open(fact: Fact) {
    let dialogRef = this.mdDialog.open(FactPreviewDialogComponent);
    dialogRef.componentInstance.fact = fact;
  }

}
