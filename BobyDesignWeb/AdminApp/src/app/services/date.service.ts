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
}