import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreModule } from '../../core';

import { ResultRoutingModule } from './result-routing.module';
import { ResultComponent } from './result.component';

@NgModule({
  imports: [
    CommonModule,
    ResultRoutingModule,
    CoreModule
  ],
  declarations: [ResultComponent]
})
export class ResultModule { }
