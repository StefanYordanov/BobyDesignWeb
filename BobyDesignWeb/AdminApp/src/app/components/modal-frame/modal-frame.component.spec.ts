import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalFrameComponent } from './modal-frame.component';

describe('ModalFrameComponent', () => {
  let component: ModalFrameComponent<any>;
  let fixture: ComponentFixture<ModalFrameComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalFrameComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalFrameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
