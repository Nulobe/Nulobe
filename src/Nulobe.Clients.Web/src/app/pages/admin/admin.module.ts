import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';

import { AppRoutingModule } from './admin-routing.module';

import { AuthModule } from '../../features/auth/auth.module';

@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    AuthModule
  ],
  declarations: [AdminComponent]
})
export class AdminModule { }
