import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerPickerComponent } from './customer-picker.component';

describe('CustomerPickerComponent', () => {
  let component: CustomerPickerComponent;
  let fixture: ComponentFixture<CustomerPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerPickerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
