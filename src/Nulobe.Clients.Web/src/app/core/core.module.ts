import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApiModule } from './api';
import { FactsModule, FactListComponent, FactFormBulkComponent } from './facts';
import { SpinnerModule, SpinnerComponent } from './spinner';
import { TagsModule, TagListComponent, TagSelectorComponent } from './tags';

@NgModule({
  imports: [
    CommonModule,
    ApiModule,
    FactsModule,
    SpinnerModule,
    TagsModule
  ],
  exports: [
    FactListComponent, FactFormBulkComponent,
    SpinnerComponent,
    TagListComponent, TagSelectorComponent
  ]
})
export class CoreModule { }
