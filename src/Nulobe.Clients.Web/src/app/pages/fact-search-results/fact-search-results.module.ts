import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdIconModule, MdButtonModule, MdDialogModule } from '@angular/material';

import { CoreModule } from '../../core';
import { AuthModule } from '../../features/auth';
import { FactListModule } from '../_fact-list';

import { FactSearchResultsRoutingModule } from './fact-search-results-routing.module';
import { FactSearchResultsComponent } from './fact-search-results.component';
import { ExportResultsDialogComponent } from './pages/export-results-dialog/export-results-dialog.component';
import { ExportResultsDialogService } from './pages/export-results-dialog/export-results-dialog.service';
import { ExportService } from './services/export';
import { ExportNotificationService } from './services/export-notification/export-notification.service';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    FactListModule,
    MdIconModule,
    MdButtonModule,
    MdDialogModule,
    FactSearchResultsRoutingModule,
    AuthModule
  ],
  declarations: [FactSearchResultsComponent, ExportResultsDialogComponent],
  entryComponents: [ExportResultsDialogComponent],
  providers: [
    ExportResultsDialogService,
    ExportService,
    ExportNotificationService
  ]
})
export class FactSearchResultsModule { }
