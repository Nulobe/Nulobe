import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { MdButtonModule, MdIconModule } from '@angular/material';
import { TagInputModule } from 'ngx-chips';

import { HomeRoutingModule } from './home-routing.module';
import { TagsModule } from './../../features/tags/tags.module';

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
    TagsModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
