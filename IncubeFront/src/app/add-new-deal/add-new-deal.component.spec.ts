import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewDealComponent } from './add-new-deal.component';

describe('AddNewDealComponent', () => {
  let component: AddNewDealComponent;
  let fixture: ComponentFixture<AddNewDealComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddNewDealComponent]
    });
    fixture = TestBed.createComponent(AddNewDealComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
