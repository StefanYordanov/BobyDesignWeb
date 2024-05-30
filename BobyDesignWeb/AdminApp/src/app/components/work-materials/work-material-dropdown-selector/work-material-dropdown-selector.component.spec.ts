import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkMaterialDropdownSelectorComponent } from './work-material-dropdown-selector.component';

describe('WorkMaterialDropdownSelectorComponent', () => {
  let component: WorkMaterialDropdownSelectorComponent;
  let fixture: ComponentFixture<WorkMaterialDropdownSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkMaterialDropdownSelectorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkMaterialDropdownSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
