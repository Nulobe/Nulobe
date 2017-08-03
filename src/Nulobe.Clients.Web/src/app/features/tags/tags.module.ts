import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { TagInputModule } from 'ngx-chips';
import { TagSelectorComponent } from './tag-selector/tag-selector.component';

import { ApiModule } from '../api/api.module';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    FormsModule,
    TagInputModule,
    ApiModule
  ],
  declarations: [TagSelectorComponent],
  exports: [TagSelectorComponent]
})
export class TagsModule { }
