import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { TagSelectorComponent } from '../../core/tags';
import { AuthService } from '../../features/auth';

import { TagEncodingHelper } from '../fact-search-results';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  placeholder = 'Search via tags e.g. "dairy"';
  searchTags: string[] = [];

  @ViewChild(TagSelectorComponent) tagSelector: TagSelectorComponent;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    setTimeout(() => this.tagSelector.focus(), 500);
  }

  updateTags(tags: string[]) {
    this.searchTags = tags;
  }

  search() {
    if (this.searchTags.length) {
      this.router.navigate([`q/${TagEncodingHelper.encode(this.searchTags)}`]);
    }
  }
}
