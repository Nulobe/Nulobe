import { TestBed, inject } from '@angular/core/testing';

import { FactQueryService } from './fact-query.service';

describe('FactQueryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FactQueryService]
    });
  });

  it('should be created', inject([FactQueryService], (service: FactQueryService) => {
    expect(service).toBeTruthy();
  }));
});
