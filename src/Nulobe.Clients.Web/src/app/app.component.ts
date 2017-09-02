import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  private isHome$: Observable<boolean>;

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
    this.router.errorHandler = (err) => {
      console.error(err);
    };

    this.router.events.subscribe(x => {
      console.log(x);
    });

    this.isHome$ = this.router.events
      .filter(e => e instanceof NavigationEnd)
      .map((e: NavigationEnd) => {
        let urlSplit = e.url.split('/');
        return urlSplit.length === 1 || urlSplit[1] === '';
      });
  }
}
