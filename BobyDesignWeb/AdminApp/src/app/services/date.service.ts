import { Injectable } from "@angular/core";
import { DateOnlyModel } from "../models/common.model";

@Injectable({
    providedIn: 'root',
  })
export class DateService{
   dateOnlyToString(dateOnly: DateOnlyModel) {
    const date = new Date(dateOnly.year, dateOnly.month - 1, dateOnly.day)
    return date.toLocaleDateString('en-GB', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
      });
   }
   dateToString(date: Date) {
    return this.dateOnlyToString({
      day: date.getDate(),
      month: date.getMonth() + 1,
      year: date.getFullYear()
    })
    // const date = new Date(dateOnly.year, dateOnly.month - 1, dateOnly.day)
    // return date.toLocaleDateString('en-GB', {
    //     year: 'numeric',
    //     month: '2-digit',
    //     day: '2-digit'
    //   });
   }
   stringToDateOnly(stringDate: string) : DateOnlyModel {
    return this.stringToDateOnlyByFormat(stringDate, 'dd/MM/yyyy', '/')
   }

   private stringToDateOnlyByFormat(_date: string,_format: string,_delimiter: string): DateOnlyModel
   {
               const formatLowerCase=_format.toLowerCase();
               const formatItems=formatLowerCase.split(_delimiter);
               const dateItems=_date.split(_delimiter);
               const monthIndex=formatItems.indexOf("mm");
               const dayIndex=formatItems.indexOf("dd");
               const yearIndex=formatItems.indexOf("yyyy");
               return {
                year: Number(dateItems[yearIndex]),
                month: Number(dateItems[monthIndex]),
                day: Number(dateItems[dayIndex])
               }
   }
}