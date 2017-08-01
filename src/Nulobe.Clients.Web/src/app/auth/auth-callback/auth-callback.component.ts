import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Auth0AuthService } from '../auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private authService: Auth0AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.authService.onLoginCallback();
    this.router.navigate(['/']);
  }
}
