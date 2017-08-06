import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SpinnerComponent } from './spinner/spinner.component';
import { DebounceNavigationGuard } from './debounce-navigation/debounce-navigation.guard';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [DebounceNavigationGuard],
  declarations: [SpinnerComponent],
  exports: [SpinnerComponent]
})
export class CoreModule { }
