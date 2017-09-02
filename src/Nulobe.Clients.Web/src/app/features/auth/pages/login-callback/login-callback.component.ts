import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../../service/auth.service';

@Component({
  selector: 'auth-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.scss']
})
export class LoginCallbackComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.authService.onLoginCallback(params['authorityName']).then(() => {
        let redirect = localStorage.getItem('login:redirect');
        this.router.navigateByUrl(redirect || '');
      });
    });
  }
}
