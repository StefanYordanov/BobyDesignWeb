import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerReviewIconComponent } from './customer-review-icon.component';

describe('CustomerReviewIconComponent', () => {
  let component: CustomerReviewIconComponent;
  let fixture: ComponentFixture<CustomerReviewIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerReviewIconComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerReviewIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
