import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CalendarCell } from 'src/app/models/calendar.model';
import { DateOnlyModel } from 'src/app/models/common.model';
import { Order, OrderStatus } from 'src/app/models/order.model';
import { CalendarService } from 'src/app/services/calendar.service';
import { CustomersService } from 'src/app/services/customers.service';
import { DateService } from 'src/app/services/date.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.scss']
})
export class CalendarViewComponent implements OnInit {

  @Input() isPicker = false;
  @Output() dateChanged = new EventEmitter<DateOnlyModel>();

  orderStatus = OrderStatus;

  year: number;
  month: number;
  calendarCells?: CalendarCell[];
  selectedDate?: DateOnlyModel;
  groupedOrders?: {
    [key: string]: Order[]
  }
  
  constructor(private calendarService: CalendarService, public dateService: DateService, private ordersService: OrdersService, public customersService: CustomersService) { 
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
    if(this.selectedDate === calendarCell.date) {
      this.selectedDate = undefined
    } else{
      this.selectedDate = calendarCell.date;
    }
    
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
    const fromDate = this.dateService.dateOnlyToString(this.calendarCells[0].date);
    const toDate = this.dateService.dateOnlyToString(this.calendarCells[this.calendarCells.length - 1].date);
    const orders = await this.ordersService.getOrders({fromDate, toDate});
    if(orders) {
      this.groupedOrders = orders.reduce((group: {[key: string]: Order[]}, item) => {
        const dateString = this.dateService.dateOnlyToString(item.finishingDate);
        if (!group[dateString]) {
         group[dateString] = [];
        }
        group[dateString].push(item);
        return group;
       }, {});
    }
  }
}
