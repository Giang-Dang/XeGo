namespace XeGo.Shared.Lib.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime ConvertVietnamTimeToUtc(DateTime vietnamDateTime)
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(vietnamDateTime, vietnamTimeZone);

            return utcDateTime;
        }

        public static DateTime ConvertUtcToVietnamTime(DateTime utcDateTime)
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vietnamTimeZone);

            return localDateTime;
        }
    }
}
