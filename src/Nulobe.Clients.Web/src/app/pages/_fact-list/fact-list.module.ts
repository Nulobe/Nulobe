import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FactListComponent } from './fact-list.component';

import { CoreModule } from '../../core';

@NgModule({
  imports: [
    CommonModule,
    CoreModule
  ],
  declarations: [FactListComponent],
  exports: [FactListComponent]
})
export class FactListModule { }
