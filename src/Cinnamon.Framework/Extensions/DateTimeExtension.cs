namespace Cinnamon.Framework.Extensions.DateTimeExtension;

public static class DateTimeExtension 
{
    public static DateTime LastDayOfMonth(this DateTime date)
    {
        date = date.AddMonths(1);
        date = new DateTime(date.Year, date.Month, 1);
        return date.AddDays(-1);
    }

    public static DateTime FirstDayOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);
}