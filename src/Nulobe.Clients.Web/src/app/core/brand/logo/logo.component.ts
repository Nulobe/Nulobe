import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'core-logo',
  templateUrl: './logo.component.html',
  styleUrls: ['./logo.component.scss']
})
export class LogoComponent implements OnInit {
  @Input() widthPixels: number;

  constructor() { }

  ngOnInit() {
    this.widthPixels = this.widthPixels || 200;
  }

}
