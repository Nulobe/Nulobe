import { Component, OnInit } from '@angular/core';

import { Fact } from '../../../../core/api';

@Component({
  selector: 'admin-fact-preview-dialog',
  templateUrl: './fact-preview-dialog.component.html',
  styleUrls: ['./fact-preview-dialog.component.scss']
})
export class FactPreviewDialogComponent implements OnInit {

  fact: Fact;

  constructor() { }

  ngOnInit() {
  }

}
