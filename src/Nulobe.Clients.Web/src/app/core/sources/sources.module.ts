import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MdInputModule, MdSelectModule } from '@angular/material';

import { SourceFormListComponent } from './source-form-list/source-form-list.component';
import { SourceFormComponent } from './source-form/source-form.component';
import { SourceDateComponent } from './source-date/source-date.component';
import { SourceJournalComponent } from './source-journal/source-journal.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MdInputModule,
    MdSelectModule
  ],
  declarations: [
    SourceFormListComponent,
    SourceFormComponent,
    SourceDateComponent,
    SourceJournalComponent
  ],
  exports: [
    SourceFormListComponent,
    SourceFormComponent
  ]
})
export class SourcesModule { }
