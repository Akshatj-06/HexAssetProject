import { TestBed } from '@angular/core/testing';

import { AuditRequestService } from './audit-request.service';

describe('AuditRequestService', () => {
  let service: AuditRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuditRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
