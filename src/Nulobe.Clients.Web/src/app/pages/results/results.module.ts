import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreModule } from '../../features/core/core.module';
import { FactsModule } from '../../features/facts/facts.module';
import { TagsModule } from '../../features/tags/tags.module';

import { ResultsRoutingModule } from './results-routing.module';
import { ResultsComponent } from './results.component';
import { ResultsComponentResolve } from './results.component.resolve';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    FactsModule,
    TagsModule,
    ResultsRoutingModule
  ],
  declarations: [ResultsComponent],
  providers: [
    ResultsComponentResolve
  ]
})
export class ResultsModule { }
