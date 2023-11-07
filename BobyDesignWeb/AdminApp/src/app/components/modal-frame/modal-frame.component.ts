import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';

@Component({
  selector: 'app-modal-frame',
  templateUrl: './modal-frame.component.html',
  styleUrls: ['./modal-frame.component.scss']
})
export class ModalFrameComponent<T> implements OnInit {

@ViewChild('modalElement') modalElement: ElementRef | undefined;
@Input() title: string = '';
@Input() buttonText: string = ''
@Input() callbacks: ModalFrameCallback<T> = {};
selectedValue?: T;

opened = false;
  constructor() { }
  

  ngOnInit(): void {
  }

  openModal(){
    if(!this.modalElement) {
      return;
    }
    this.opened=true;
  }

  clickClose() {
    if(this.callbacks.onCancel) {
      this.callbacks.onCancel(this.selectedValue);
    }
    this.opened=false;
  }
  clickOk() {
    if(this.callbacks.onOk) {
      this.callbacks.onOk(this.selectedValue);
    }
    this.opened=false;
  }
}
