import { Component, OnInit } from '@angular/core';

import { Fact } from '../../../../core/api';
import { FactLinkResolver } from '../../../../core/facts';

@Component({
  selector: 'admin-fact-preview-dialog',
  templateUrl: './fact-preview-dialog.component.html',
  styleUrls: ['./fact-preview-dialog.component.scss']
})
export class FactPreviewDialogComponent implements OnInit {

  fact: Fact;
  factLinkResolver: FactLinkResolver;

  constructor() { }

  ngOnInit() {
    this.factLinkResolver = {
      resolve: (fact: Fact | string) => "javascript:void(0)"
    };
  }

}
