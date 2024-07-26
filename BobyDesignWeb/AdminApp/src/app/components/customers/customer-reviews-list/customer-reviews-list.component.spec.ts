import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerReviewsListComponent } from './customer-reviews-list.component';

describe('CustomerReviewsListComponent', () => {
  let component: CustomerReviewsListComponent;
  let fixture: ComponentFixture<CustomerReviewsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerReviewsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerReviewsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
