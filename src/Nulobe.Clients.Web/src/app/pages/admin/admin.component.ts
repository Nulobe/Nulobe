import { Component, OnInit } from '@angular/core';

import { FactApiClient } from '../../features/api/api.swagger';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  constructor(
    private factApiClient: FactApiClient
  ) { }

  ngOnInit() {
    this.factApiClient.create({
      title: 'Test fact',
      definition: 'Test definition',
      sources: [],
      tags: ['dairy', 'nutrition'],
      credit: '@mushimas'
    })
    .subscribe((...args) => {
    });
  }

}
