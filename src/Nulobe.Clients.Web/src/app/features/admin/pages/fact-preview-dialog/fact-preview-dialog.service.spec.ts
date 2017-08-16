import { TestBed, inject } from '@angular/core/testing';

import { FactPreviewDialogService } from './fact-preview-dialog.service';

describe('FactPreviewDialogService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FactPreviewDialogService]
    });
  });

  it('should be created', inject([FactPreviewDialogService], (service: FactPreviewDialogService) => {
    expect(service).toBeTruthy();
  }));
});
