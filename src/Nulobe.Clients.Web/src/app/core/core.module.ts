import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApiModule } from './api';
import { FactsModule, FactListComponent, FactListItemComponent, FactFormBulkComponent, FactFormComponent } from './facts';
import { SpinnerModule, SpinnerComponent } from './spinner';
import { TagsModule, TagListComponent, TagSelectorComponent } from './tags';
import { SourcesModule, SourceFormListComponent } from './sources';
import { BrandModule, LogoComponent } from './brand';

@NgModule({
  imports: [
    CommonModule,
    ApiModule,
    FactsModule,
    SpinnerModule,
    TagsModule,
    SourcesModule,
    BrandModule
  ],
  exports: [
    FactListComponent, FactListItemComponent, FactFormBulkComponent, FactFormComponent,
    SpinnerComponent,
    TagListComponent, TagSelectorComponent,
    SourceFormListComponent,
    LogoComponent
  ],
  declarations: []
})
export class CoreModule { }
