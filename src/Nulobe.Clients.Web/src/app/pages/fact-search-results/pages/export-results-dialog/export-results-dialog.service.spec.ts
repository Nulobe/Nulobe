import { TestBed, inject } from '@angular/core/testing';

import { ExportResultsDialogService } from './export-results-dialog.service';

describe('ExportResultsDialogService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExportResultsDialogService]
    });
  });

  it('should be created', inject([ExportResultsDialogService], (service: ExportResultsDialogService) => {
    expect(service).toBeTruthy();
  }));
});
