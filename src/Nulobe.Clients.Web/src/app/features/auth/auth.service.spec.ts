import { TestBed, inject } from '@angular/core/testing';

import { Auth0AuthService } from './auth.service';

describe('AuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Auth0AuthService]
    });
  });

  it('should be created', inject([Auth0AuthService], (service: Auth0AuthService) => {
    expect(service).toBeTruthy();
  }));
});
