import { TestBed } from '@angular/core/testing';

import { AssetRequestService } from './asset-request.service';

describe('AssetRequestService', () => {
  let service: AssetRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssetRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
