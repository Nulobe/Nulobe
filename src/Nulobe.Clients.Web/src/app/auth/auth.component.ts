import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
  selector: 'auth',
  template: ''
})
export class AuthComponent implements OnInit {

    constructor(
        private authService: AuthService 
    ) { }

    ngOnInit() {
        this.authService.login();
    }
}