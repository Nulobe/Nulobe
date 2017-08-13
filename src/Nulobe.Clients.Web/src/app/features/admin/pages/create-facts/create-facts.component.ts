import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-facts',
  templateUrl: './create-facts.component.html',
  styleUrls: ['./create-facts.component.scss']
})
export class CreateFactsComponent implements OnInit {

  private valid: boolean;

  constructor() { }

  ngOnInit() {
    this.valid = false;
  }

}
