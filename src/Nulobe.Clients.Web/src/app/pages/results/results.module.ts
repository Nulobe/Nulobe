import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreModule } from '../../core';
import { AuthModule } from '../../features/auth';

import { ResultsRoutingModule } from './results-routing.module';
import { ResultsComponent } from './results.component';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    ResultsRoutingModule,
    AuthModule
  ],
  declarations: [ResultsComponent],
  providers: []
})
export class ResultsModule { }
