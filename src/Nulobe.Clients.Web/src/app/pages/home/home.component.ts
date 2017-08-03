import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Auth0AuthService } from '../../auth/auth.service';

import { ResultsPathHelper } from '../results/results-path.helper';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  private searchTags: string[] = [];

  constructor(
    private authService: Auth0AuthService,
    private router: Router
  ) { }

  ngOnInit() {
  }

  updateTags(tags: string[]) {
    this.searchTags = tags;
  }

  search() {
    if (this.searchTags.length) {
      this.router.navigate([ResultsPathHelper.encode(this.searchTags)]);
    }
  }
}
