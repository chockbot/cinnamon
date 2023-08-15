using System.Security.AccessControl;

namespace Cinnamon.Framework.Helpers;

public class DateNextPeriod 
{
    private readonly DateTime startDate;
    private readonly Period period;
    private readonly int periodCount;

    private DateTime currentPeriod;
    private DateTime endPeriod;

    public DateNextPeriod(DateTime startDate, Period period, int periodCount)
    {
        this.startDate = startDate;
        this.period = period;
        this.periodCount = periodCount;

        this.currentPeriod = startDate;
    }

    public DateTime PeriodStart 
    {
        get {
            return currentPeriod;
        }
    }

    public DateTime PeriodEnd 
    {
        get 
        {
            return CalculatePeriod(currentPeriod);
        }
    }
    

    public DateNextPeriod NextPeriod()
    {
        currentPeriod = CalculatePeriod(currentPeriod).AddDays(1);
        return this;
    }

    private DateTime CalculatePeriod(DateTime dateStart)
    {
        DateTime result = dateStart;
        switch(period)
        {
            case Period.Day:
                result = dateStart.AddDays(periodCount);
                break;
            case Period.Week:
                result = dateStart.AddDays(periodCount * 7);
                break;
            case Period.Month:
                result = dateStart.AddMonths(periodCount);
                break;
        }
        return result;
    }

    public enum Period 
    {
        Day = 0,
        Week = 1,
        Month = 2
    }

    public static DateNextPeriod CreateRecurring(DateTime startDate, Period period, int periodCount)
    {
        var datePeriod = new DateNextPeriod(startDate, period, periodCount);
        var now = DateTime.Now;

        while(datePeriod.PeriodEnd.Date < now.Date)
        {
            datePeriod.NextPeriod();
        }
        return datePeriod;
    }
}