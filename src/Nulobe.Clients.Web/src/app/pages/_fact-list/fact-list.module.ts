import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FactListComponent } from './fact-list.component';

import { CoreModule } from '../../core';
import { AuthModule } from '../../features/auth';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    AuthModule
  ],
  declarations: [FactListComponent],
  exports: [FactListComponent]
})
export class FactListModule { }
