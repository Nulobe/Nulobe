import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { MdChipsModule } from '@angular/material';
import { TagInputModule } from '../../imports';

import { TagSelectorComponent } from './tag-selector/tag-selector.component';
import { TagListComponent } from './tag-list/tag-list.component';

import { ApiModule } from '../api/api.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MdChipsModule,
    TagInputModule,
    ApiModule
  ],
  declarations: [
    TagSelectorComponent,
    TagListComponent
  ],
  exports: [
    TagSelectorComponent,
    TagListComponent
  ]
})
export class TagsModule { }
