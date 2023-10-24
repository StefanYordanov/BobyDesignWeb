import { Injectable } from "@angular/core";
import { CalendarCell } from "../models/calendar.model";

@Injectable({
    providedIn: 'root',
  })
export class CalendarService{
    getMonthCells(year: number, month: number): CalendarCell[] {
        const result: Date[] = [];
        const monthStart = new Date(year, month, 1);
        for(let currentDate = monthStart; currentDate.getMonth() == month; currentDate.setDate(currentDate.getDate() + 1)){
            result.push(new Date(currentDate));
        }

        const sundayDoW = 0;
        const mondaryDoW = 1;
        while(result[0].getDay() !== mondaryDoW) {
            const prevDate = new Date(result[0])
            prevDate.setDate(result[0].getDate()- 1 );
            result.unshift(prevDate)
        }
        while(result[result.length - 1].getDay() !== sundayDoW) {
            const nextDate = new Date(result[result.length - 1])
            nextDate.setDate(result[result.length - 1].getDate()+ 1 );
            result.push(nextDate)
        };

        const today = (new Date()).setHours(0, 0, 0, 0);
        return result.map(d=> {
            return {
                date: d,
                withinMonthBoundaries: d.getMonth() === month,
                isToday: (new Date(d)).setHours(0, 0, 0, 0) === today
            }
        });
    }
}