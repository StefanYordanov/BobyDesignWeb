import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkMaterialsReportComponent } from './work-materials-report.component';

describe('OrdersViewComponent', () => {
  let component: WorkMaterialsReportComponent;
  let fixture: ComponentFixture<WorkMaterialsReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkMaterialsReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkMaterialsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
