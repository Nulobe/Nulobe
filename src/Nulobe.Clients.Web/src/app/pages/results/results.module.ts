import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResultsRoutingModule } from './results-routing.module';
import { ResultsComponent } from './results.component';
import { ResultsComponentResolve } from './results.component.resolve';

@NgModule({
  imports: [
    CommonModule,
    ResultsRoutingModule
  ],
  declarations: [ResultsComponent],
  providers: [
    ResultsComponentResolve
  ]
})
export class ResultsModule { }
