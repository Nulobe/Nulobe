import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdIconModule, MdButtonModule, MdDialogModule } from '@angular/material';

import { CoreModule } from '../../core';
import { AuthModule } from '../../features/auth';

import { ResultsRoutingModule } from './results-routing.module';
import { ResultsComponent } from './results.component';
import { ExportResultsDialogComponent } from './pages/export-results-dialog/export-results-dialog.component';
import { ExportResultsDialogService } from './pages/export-results-dialog/export-results-dialog.service';
import { ExportService } from './services/export';
import { ExportNotificationService } from './services/export-notification/export-notification.service';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    MdIconModule,
    MdButtonModule,
    MdDialogModule,
    ResultsRoutingModule,
    AuthModule
  ],
  declarations: [ResultsComponent, ExportResultsDialogComponent],
  entryComponents: [ExportResultsDialogComponent],
  providers: [
    ExportResultsDialogService,
    ExportService,
    ExportNotificationService
  ]
})
export class ResultsModule { }
