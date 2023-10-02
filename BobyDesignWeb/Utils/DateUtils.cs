namespace BobyDesignWeb.Utils
{
    public static class DateUtils
    {
        public static DateTime ToBulgarianDateTime(this DateTime date) 
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date,
                TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));
        }
    }
}
