import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FactApiClient } from './core/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(
    private router: Router,
    private factApiClient: FactApiClient
  ) { }

  ngOnInit(): void {
    this.router.errorHandler = (err) => {
      console.error(err);
    };

    this.router.events.subscribe(x => {
      console.log(x);
    });
  }
}
