import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdButtonModule, MdDialogModule } from '@angular/material';

import { AuthModule } from '../../features/auth';
import { CoreModule } from '../../core';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminStateModule } from './state/admin-state.module';
import { AdminComponent } from './admin.component';
import { CreateFactsComponent } from './pages/create-facts/create-facts.component';
import { FactPreviewDialogComponent } from './pages/fact-preview-dialog/fact-preview-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    MdButtonModule,
    MdDialogModule,

    CoreModule,
    AuthModule,

    AdminRoutingModule,
    AdminStateModule,
  ],
  declarations: [
    AdminComponent,
    CreateFactsComponent,
    FactPreviewDialogComponent
  ],
  entryComponents: [
    FactPreviewDialogComponent
  ]
})
export class AdminModule { }
