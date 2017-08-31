import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router, UrlSerializer } from '@angular/router';
import { MdDialogRef } from '@angular/material';

import { Fact } from '../../../../core/api';
import { AuthService, AUTH_CONSTANTS } from '../../../../features/auth';

import { ExportService, ExportTarget } from '../../services/export';
import { ExportNotificationService } from '../../services/export-notification/export-notification.service';

@Component({
  selector: 'app-export-results-dialog',
  templateUrl: './export-results-dialog.component.html',
  styleUrls: ['./export-results-dialog.component.scss']
})
export class ExportResultsDialogComponent implements OnInit {
  @Input() tags: string[];
  @Input() facts: Fact[];
  @Output() onSuccess = new EventEmitter();

  constructor(
    private authService: AuthService,
    private exportService: ExportService,
    private urlSerializer: UrlSerializer,
    private router: Router,
    private exportNotificationService: ExportNotificationService
  ) { }

  ngOnInit() {
  }

  exportToQuizlet() {
    if (this.authService.isAuthenticated(AUTH_CONSTANTS.AUTHORITY_NAMES.QUIZLET)) {
      this.exportService.exportToTarget(this.tags, ExportTarget.Quizlet)
        .then(result => {
          this.onSuccess.emit();
          setTimeout(() => {
            this.exportNotificationService.notifySuccess(ExportTarget.Quizlet, result);
          }, 500);
        });
    } else {
      this.router.navigate([`LOBE/login/${AUTH_CONSTANTS.AUTHORITY_NAMES.QUIZLET}`], {
        queryParams: {
          redirect: this.getRedirectUrl(ExportTarget.Quizlet)
        }
      })
    }
  }

  private getRedirectUrl(exportTarget: ExportTarget) {
    let urlTree = this.urlSerializer.parse(this.router.url);
    urlTree.queryParams = { export: ExportTarget.Quizlet.toString() }; 
    return this.urlSerializer.serialize(urlTree);
  }

}
