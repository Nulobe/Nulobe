import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { MdButtonModule, MdIconModule } from '@angular/material';
import { TagInputModule } from 'ngx-chips';

import { HomeRoutingModule } from './home-routing.module';
import { CoreModule } from './../../core';
import { AuthModule } from './../../features/auth/auth.module';

import { HomeComponent } from './home.component';

@NgModule({
  imports: [
    CommonModule,

    BrowserAnimationsModule,
    FormsModule,
    MdButtonModule,
    MdIconModule,
    TagInputModule,

    HomeRoutingModule,
    CoreModule,
    AuthModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
