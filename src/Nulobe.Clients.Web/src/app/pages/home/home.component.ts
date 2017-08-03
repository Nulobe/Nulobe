import { Component, OnInit } from '@angular/core';

import { Auth0AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  private searchTags: string[] = [];

  constructor(
    private authService: Auth0AuthService
  ) { }

  ngOnInit() {
  }

  updateTags(tags: string[]) {
    this.searchTags = tags;
  }

  search() {
    alert('search requested!');
  }

}
