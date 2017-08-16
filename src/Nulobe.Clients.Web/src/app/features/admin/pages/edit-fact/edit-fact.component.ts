import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Fact } from '../../../../core/api';

@Component({
  selector: 'app-edit-fact',
  templateUrl: './edit-fact.component.html',
  styleUrls: ['./edit-fact.component.scss']
})
export class EditFactComponent implements OnInit {

  private fact$: Fact;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.fact$ = this.route.data.map(d => d.fact);
  }

}
