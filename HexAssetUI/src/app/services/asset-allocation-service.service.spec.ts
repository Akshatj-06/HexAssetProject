import { TestBed } from '@angular/core/testing';

import { AssetAllocationServiceService } from './asset-allocation-service.service';

describe('AssetAllocationServiceService', () => {
  let service: AssetAllocationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssetAllocationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
