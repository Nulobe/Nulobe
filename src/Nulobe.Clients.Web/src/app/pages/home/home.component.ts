import { Component, OnInit } from '@angular/core';

import { Auth0AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(
    private authService: Auth0AuthService
  ) { }

  ngOnInit() {
  }

}
