import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit {

  @Output() pageChanged = new EventEmitter<number>()
  @Input() pagesCount: number = 1;
  @Input() currentPage: number = 1;
  pagesDisplayRange= 3;

  constructor() { }

  ngOnInit(): void {
  }

  getPagesRange(): number[] {
    const minPageNumber = Math.max(this.currentPage -this.pagesDisplayRange, 1);
    const maxPageNumber = Math.min(this.currentPage + this.pagesDisplayRange, this.pagesCount);
    const result: number[] = [];
    for(let i = minPageNumber; i <= maxPageNumber; i++) {
      result.push(i);
    }
    return result;
  }

  switchPage(pageNumber: number) {
    this.pageChanged.emit(pageNumber);
  }
}
