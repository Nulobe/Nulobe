import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AuthService } from '../../service/auth.service';

@Component({
  selector: 'app-auth',
  template: ''
})
export class AuthComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(queryParams => {
      let { redirect } = queryParams;
      if (redirect) {
        localStorage.setItem('login:redirect', redirect)
      }

      this.route.params.subscribe(params => {
        this.authService.redirectToLogin(params['authorityName']);
      });
    });
  }
}