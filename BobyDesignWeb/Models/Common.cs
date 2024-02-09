﻿using System.Globalization;

namespace BobyDesignWeb.Models
{
    public class PageViewModel<T>
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public ICollection<T> Items { get; set; } = new List<T>();
    }

    public class DateOnlyModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
    }

    public static class DateOnlyExtensions
    {
        public static DateTime ToDateTime(this DateOnlyModel model)
        {
            return new DateTime(model.Year, model.Month, model.Day);
        }

        public static DateOnlyModel ToDateOnlyModel(this DateTime date)
        {
            return new DateOnlyModel { Year = date.Year, Month = date.Month, Day = date.Day };
        }

        public static DateTime ToDateTime(this string stringDate)
        {
            return DateTime.ParseExact(stringDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
        }
    }
}
