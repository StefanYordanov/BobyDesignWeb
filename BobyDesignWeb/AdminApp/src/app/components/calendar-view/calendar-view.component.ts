import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CalendarCell } from 'src/app/models/calendar.model';
import { Order } from 'src/app/models/order.model';
import { CalendarService } from 'src/app/services/calendar.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.scss']
})
export class CalendarViewComponent implements OnInit {

  @Input() isPicker = false;
  @Output() dateChanged = new EventEmitter<Date>();

  year: number;
  month: number;
  calendarCells?: CalendarCell[];
  selectedDate?: Date;
  groupedOrders?: {
    [key: number]: Order[]
  }
  
  constructor(private calendarService: CalendarService, private ordersService: OrdersService) { 
    const date = new Date();
    this.year = date.getFullYear();
    this.month = date.getMonth();
    
  }

  async ngOnInit() {
    await this.updateCalendar();
  }

  clickCalendarCell(calendarCell: CalendarCell){
    if(!this.isPicker) {
      return;
    }
    this.selectedDate = calendarCell.date;
    this.dateChanged.emit(calendarCell.date);
  }

  setYear(year: number) {
    this.year = year;
    this.updateCalendar()
  }

  async nextMonth(){
    this.month++;
    if(this.month >=12){
      this.month = 0;
      this.year++;
    }
    await this.updateCalendar();
  }

  async previousMonth(){
    this.month--;
    if(this.month <=-1){
      this.month = 11;
      this.year--;
    }
    await this.updateCalendar();
  }

  async updateCalendar() {
    if(this.year < 1000 || this.year > 9999) {
      this.calendarCells = undefined;
      this.groupedOrders = undefined;
      return;
    }
    this.calendarCells = this.calendarService.getMonthCells(this.year, this.month);
    const fromDate = this.calendarCells[0].date.toDateString();
    const toDate = this.calendarCells[this.calendarCells.length - 1].date.toDateString();
    const orders = await this.ordersService.getOrders({fromDate, toDate});
    if(orders) {
      this.groupedOrders = orders.reduce((group: {[key: number]: Order[]}, item) => {
        if (!group[item.finishingDate.getTime()]) {
         group[item.finishingDate.getTime()] = [];
        }
        group[item.finishingDate.getTime()].push(item);
        return group;
       }, {});
    }
  }
}
