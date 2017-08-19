import { Component, OnInit } from '@angular/core';

import { FactApiClient } from '../../core/api';
import { AuthService } from '../../features/auth';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  constructor(
    private factApiClient: FactApiClient,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  logout() {
    this.authService.logout();
  }

}
