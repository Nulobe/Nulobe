import { Component, OnInit } from '@angular/core';
import { Auth0AuthService } from './auth.service';

@Component({
  selector: 'auth',
  template: ''
})
export class AuthComponent implements OnInit {

    constructor(
        private authService: Auth0AuthService 
    ) { }

    ngOnInit() {
        this.authService.login();
    }
}