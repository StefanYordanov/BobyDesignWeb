import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss']
})
export class IconComponent implements OnInit {

  @Input() iconName: string = '';
  @Input() size: number = 16;

  constructor() { }

  ngOnInit(): void {
  }

  iconClasses(): string {
    return `bi bi-${this.iconName}`;
  }
}
