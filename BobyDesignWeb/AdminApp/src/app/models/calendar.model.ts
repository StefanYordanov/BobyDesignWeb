import { DateOnlyModel } from "./common.model";

export interface CalendarCell{
    date: DateOnlyModel;
    withinMonthBoundaries: boolean;
    isToday: boolean;
}