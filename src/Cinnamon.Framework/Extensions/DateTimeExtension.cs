namespace Cinnamon.Framework.Extensions.DateTimeExtension;

public static class DateTimeExtension 
{
    public static DateTime LastDayOfMonth(this DateTime date)
        => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

    public static DateTime FirstDayOfMonth(this DateTime date) 
        => new DateTime(date.Year, date.Month, 1);
}