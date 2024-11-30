import { TestBed } from '@angular/core/testing';

import { containerdetailsService } from './container-details.service';

describe('ContainerDetailsService', () => {
  let service: containerdetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(containerdetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
