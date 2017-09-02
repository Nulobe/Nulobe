import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FactListModule } from '../_fact-list';

import { FactRoutingModule } from './fact-routing.module';
import { FactComponent } from './fact.component';

@NgModule({
  imports: [
    CommonModule,
    FactRoutingModule,
    FactListModule
  ],
  declarations: [FactComponent]
})
export class FactModule { }
