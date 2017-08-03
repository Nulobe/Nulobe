import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreModule } from '../../features/core/core.module'

import { ResultsRoutingModule } from './results-routing.module';
import { ResultsComponent } from './results.component';
import { ResultsComponentResolve } from './results.component.resolve';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    ResultsRoutingModule
  ],
  declarations: [ResultsComponent],
  providers: [
    ResultsComponentResolve
  ]
})
export class ResultsModule { }
