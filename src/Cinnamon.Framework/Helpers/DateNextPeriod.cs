using System.Security.AccessControl;

namespace Cinnamon.Framework.Helpers;

public class DateNextPeriod 
{
    private readonly DateTime startDate;
    private readonly Period period;
    private readonly int periodCount;

    private DateTime currentPeriod;

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

    public static DateNextPeriod CreateRecurring(DateTime startDate, string sessionName)
    {
        var period = DateNextPeriod.Period.Day;
        var periodCount = 0;
        switch (sessionName)
        {
            case "2 Weeks":
                period = DateNextPeriod.Period.Week;
                periodCount = 2;
                break;
            case "3 Weeks":
                period = DateNextPeriod.Period.Week;
                periodCount = 3;
                break;
            case "1 Month":
                period = DateNextPeriod.Period.Month;
                periodCount = 1;
                break;
            case "2 Months":
                period = DateNextPeriod.Period.Month;
                periodCount = 2;
                break;
            case "3 Months":
                period = DateNextPeriod.Period.Month;
                periodCount = 3;
                break;
        }
        var datePeriod = new DateNextPeriod(startDate, period, periodCount);
        var now = DateTime.Now;

        while(datePeriod.PeriodEnd.Date < now.Date)
        {
            datePeriod.NextPeriod();
        }
        return datePeriod;
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