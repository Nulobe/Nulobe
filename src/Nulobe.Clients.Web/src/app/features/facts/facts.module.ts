import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MdIconModule } from '@angular/material';

import { FactListComponent } from './fact-list/fact-list.component';
import { FactListItemComponent } from './fact-list-item/fact-list-item.component';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    MdIconModule
  ],
  declarations: [
    FactListComponent,
    FactListItemComponent
  ],
  exports: [FactListComponent]
})
export class FactsModule { }
