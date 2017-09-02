import { Injectable } from '@angular/core';

import { QuizletSet } from '../../../../core/api';

import { ExportTarget } from '../export';

@Injectable()
export class ExportNotificationService {

  constructor() { }

  notifySuccess(target: ExportTarget, result: any) {
    setTimeout(() => alert(`Successfully exported results to ${this.getTargetName(target)}`));
    window.open(this.getUrl(target, result));
  }

  private getTargetName(target: ExportTarget): string {
    switch (target) {
      case ExportTarget.Quizlet: return 'Quizlet';
      default:
        throw new Error('Unrecognised export target');
    }
  }

  private getUrl(target: ExportTarget, result: any): string {
    switch (target) {
      case ExportTarget.Quizlet:
        return (<QuizletSet>result).url;
      default:
        throw new Error('Unrecognised export target');
    }
  }

}
