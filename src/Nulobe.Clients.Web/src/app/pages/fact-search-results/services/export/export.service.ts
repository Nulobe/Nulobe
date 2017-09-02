import { Injectable } from '@angular/core';

import { QuizletApiClient, QuizletSet } from '../../../../core/api';
import { AuthService, AUTH_CONSTANTS } from '../../../../features/auth';

import { ExportTarget } from './export-target';

@Injectable()
export class ExportService {

  constructor(
    private authService: AuthService,
    private quizletApiClient: QuizletApiClient
  ) { }

  exportToTarget(tags: string[], target: ExportTarget): Promise<any> {
    switch (target) {
      case ExportTarget.Quizlet:
        return this.exportToQuizlet(tags);
      default:
        throw new Error('Unrecognised export target');
    }
  }

  private exportToQuizlet(tags: string[]): Promise<QuizletSet> {
    if (!this.authService.isAuthenticated(AUTH_CONSTANTS.AUTHORITY_NAMES.QUIZLET)) {
      throw new Error('Authentication required before exporting to quizlet');
    }

    return this.quizletApiClient
      .createSet({
        tags: tags.join(','),
        pageSize: '50'
      })
      .toPromise();
  }
}
