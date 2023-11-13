import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkMaterialPickerComponent } from './work-material-picker.component';

describe('WorkMaterialPickerComponent', () => {
  let component: WorkMaterialPickerComponent;
  let fixture: ComponentFixture<WorkMaterialPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkMaterialPickerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkMaterialPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
