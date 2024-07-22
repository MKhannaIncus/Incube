import { TestBed } from '@angular/core/testing';

import { FinancialMetricsService } from './financial-metrics.service';

describe('FinancialMetricsService', () => {
  let service: FinancialMetricsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FinancialMetricsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
