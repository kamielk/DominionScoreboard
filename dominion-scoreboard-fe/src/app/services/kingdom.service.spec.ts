import { TestBed } from '@angular/core/testing';

import { KingdomService } from './kingdom.service';

describe('KingdomService', () => {
  let service: KingdomService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KingdomService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
