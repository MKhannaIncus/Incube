import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DealDataComponent } from './deal-data.component';

describe('DealDataComponent', () => {
  let component: DealDataComponent;
  let fixture: ComponentFixture<DealDataComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DealDataComponent]
    });
    fixture = TestBed.createComponent(DealDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
