import { TestBed, inject } from '@angular/core/testing';

import { ExportNotificationService } from './export-notification.service';

describe('ExportNotificationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExportNotificationService]
    });
  });

  it('should be created', inject([ExportNotificationService], (service: ExportNotificationService) => {
    expect(service).toBeTruthy();
  }));
});
