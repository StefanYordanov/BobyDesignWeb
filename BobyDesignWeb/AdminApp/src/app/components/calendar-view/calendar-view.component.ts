import { Component, OnInit } from '@angular/core';
import { CalendarCell } from 'src/app/models/calendar.model';
import { CalendarService } from 'src/app/services/calendar.service';

@Component({
  selector: 'app-calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.scss']
})
export class CalendarViewComponent implements OnInit {

  year: number;
  month: number;
  calendarCells: CalendarCell[] = [];
  
  constructor(private calendarService: CalendarService) { 
    const date = new Date();
    this.year = date.getFullYear();
    this.month = date.getMonth();
    this.updateCalendar();
  }

  ngOnInit(): void {
  }

  createRandomRange(maxRange: number): number[] {

    const number = Math.floor(Math.random() * (maxRange + 1));
    return new Array(number).fill(0)
      .map((n, index) => index + 1);
  }

  updateCalendar() {
    this.calendarCells = this.calendarService.getMonthCells(this.year, this.month);
  }
}
