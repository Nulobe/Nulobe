import { Component, OnInit } from '@angular/core';
import { MdDialog } from '@angular/material';

import { Fact } from '../../../../core/api';

import { FactPreviewDialogComponent } from '../fact-preview-dialog/fact-preview-dialog.component';

@Component({
  selector: 'app-create-facts',
  templateUrl: './create-facts.component.html',
  styleUrls: ['./create-facts.component.scss']
})
export class CreateFactsComponent implements OnInit {

  private fact: Fact;
  private valid: boolean;

  constructor(
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

}
