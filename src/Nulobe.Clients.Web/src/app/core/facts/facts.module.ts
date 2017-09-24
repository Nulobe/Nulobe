import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { MdIconModule, MdInputModule, MdSelectModule } from '@angular/material';
import { MarkdownModule } from 'angular2-markdown';

import { TagsModule } from '../tags';
import { SourcesModule } from '../sources';
import { BrandModule } from '../brand';

import { FactListComponent } from './fact-list/fact-list.component';
import { FactListItemComponent } from './fact-list-item/fact-list-item.component';
import { FactFormBulkComponent } from './fact-form-bulk/fact-form-bulk.component';
import { FactFormComponent } from './fact-form/fact-form.component';

import { FactQueryService } from './fact-query/fact-query.service';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MdIconModule,
    MdInputModule,
    MdSelectModule,
    MarkdownModule,
    TagsModule,
    SourcesModule,
    BrandModule
  ],
  declarations: [
    FactListComponent,
    FactListItemComponent,
    FactFormBulkComponent,
    FactFormComponent
  ],
  exports: [
    FactListComponent,
    FactListItemComponent,
    FactFormBulkComponent,
    FactFormComponent
  ],
  providers: [
    FactQueryService
  ]
})
export class FactsModule { }
