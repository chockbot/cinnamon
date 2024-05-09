using Cinnamon.Framework.Extensions.DateTimeExtension;

namespace Cinnamon.Framework.Helpers;

public class GenerateRecurrenceDate 
{
    private IDictionary<DayOfWeek, string> ToDictionaryDayWeek(string selectedDays)
    {
        IDictionary<DayOfWeek, string> result = new Dictionary<DayOfWeek, string>();
        var days = selectedDays.Split("|");

        foreach(var day in days)
        {
            DayOfWeek dw = day switch {
                "MON" => DayOfWeek.Monday,
                "TUE" => DayOfWeek.Tuesday,
                "WED" => DayOfWeek.Wednesday,
                "THU" => DayOfWeek.Thursday,
                "FRI" => DayOfWeek.Friday,
                "SAT" => DayOfWeek.Saturday,
                _ => DayOfWeek.Sunday
            };

            if(!result.ContainsKey(dw))
            {
                result.Add(dw, day);
            }
        }

        return result;
    }

    public IEnumerable<DateItem> GenerateWeekday(int repeat, DateTime start, DateTime end, 
        TimeSpan timeDuration, TimeSpan timeStart)
    {
        IList<DateItem> generatedDates = new List<DateItem>();

        int daysToSkip = (repeat - 1) * 7;

        DateTime recurringDate = start;
        while(recurringDate <= end)
        {
            var item = new DateItem {
                            Date = recurringDate.Date,
                            DateStart = recurringDate.Date.Add(timeStart),
                            DateEnd = recurringDate.Date.Add(timeStart).Add(timeDuration)
                        };

            switch(recurringDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    generatedDates.Add(item);
                    break;
                case DayOfWeek.Tuesday:
                    generatedDates.Add(item);
                    break;
                case DayOfWeek.Wednesday:
                    generatedDates.Add(item);
                    break;
                case DayOfWeek.Thursday:
                    generatedDates.Add(item);
                    break;
                case DayOfWeek.Friday:
                    generatedDates.Add(item);
                    break;
                case DayOfWeek.Sunday:
                    recurringDate = recurringDate.AddDays(daysToSkip);
                    break;
            }

            recurringDate = recurringDate.AddDays(1);
        }

        return generatedDates;
    }

    public IEnumerable<DateItem> GenerateDaily(int repeat, DateTime start, DateTime end, 
        TimeSpan timeDuration, TimeSpan timeStart)
    {
        IList<DateItem> generatedDates = new List<DateItem>();

        int daysToSkip = repeat;

        DateTime recurringDate = start;
        while(recurringDate <= end)
        {
            var item = new DateItem {
                            Date = recurringDate.Date,
                            DateStart = recurringDate.Date.Add(timeStart),
                            DateEnd = recurringDate.Date.Add(timeStart).Add(timeDuration)
                        };
            

            generatedDates.Add(item);

            recurringDate = recurringDate.AddDays(daysToSkip);
        }

        return generatedDates;
    }

    public IEnumerable<DateItem> GenerateWeekly(int repeat, DateTime start, DateTime end, 
        TimeSpan timeDuration, TimeSpan timeStart, string selectedDays)
    {
        IList<DateItem> generatedDates = new List<DateItem>();

        int daysToSkip = (repeat - 1) * 7;
        
        var dayWeeks = ToDictionaryDayWeek(selectedDays);

        DateTime recurringDate = start;
        while(recurringDate <= end)
        {
            var item = new DateItem {
                            Date = recurringDate.Date,
                            DateStart = recurringDate.Date.Add(timeStart),
                            DateEnd = recurringDate.Date.Add(timeStart).Add(timeDuration)
                        };

            if(dayWeeks.ContainsKey(recurringDate.DayOfWeek))
            {
                generatedDates.Add(item);
            }

            if(recurringDate.DayOfWeek == DayOfWeek.Sunday)
            {
                recurringDate = recurringDate.AddDays(daysToSkip);
            }

            recurringDate = recurringDate.AddDays(1);
        }

        return generatedDates;
    }

    public IEnumerable<DateItem> GenerateMonthly(int repeat, DateTime start, DateTime end, 
        TimeSpan timeDuration, TimeSpan timeStart, int monthSelection, int onTheDay, 
        string monthRepeat, string monthDay)
    {
        IList<DateItem> generatedDates = new List<DateItem>();

        int monthsToSkip = repeat;

        DateTime recurringDate = start.FirstDayOfMonth();
        DateTime recurringDateEnd = end.LastDayOfMonth();

        if(monthSelection == 1)
        {
            while (recurringDate <= recurringDateEnd)
            {
                int day = Math.Min(onTheDay, DateTime.DaysInMonth(recurringDate.Year, recurringDate.Month));
                DateTime adjustedDate = new DateTime(recurringDate.Year, recurringDate.Month, day);

                if (adjustedDate.Date < start.Date)
                {
                    recurringDate = recurringDate.AddMonths(monthsToSkip);
                    continue;
                }
                
                generatedDates.Add(new DateItem
                {
                    Date = adjustedDate.Date,
                    DateStart = adjustedDate.Date.Add(timeStart),
                    DateEnd = adjustedDate.Date.Add(timeStart).Add(timeDuration)
                });
                recurringDate = recurringDate.AddMonths(monthsToSkip);
            }
        }

        if(monthSelection == 2)
        {
            DayOfWeek dw = monthDay switch {
                "monday" => DayOfWeek.Monday,
                "tuesday" => DayOfWeek.Tuesday,
                "wednesday" => DayOfWeek.Wednesday,
                "thursday" => DayOfWeek.Thursday,
                "friday" => DayOfWeek.Friday,
                "saturday" => DayOfWeek.Saturday,
                _ => DayOfWeek.Sunday
            };

            int weeksToSkip = monthRepeat switch {
                "first" => 1,
                "second" => 2,
                "third" => 3,
                "fourth" => 4,
                _ => 5
            };

            int skipCounter = 0;

            while(recurringDate <= recurringDateEnd)
            {
                do
                {
                    if(recurringDate.DayOfWeek == dw)
                    {
                        skipCounter++;
                    }

                    // skip current month and proceed to next month if date is not in range
                    if(skipCounter == weeksToSkip && recurringDate.Date < start.Date)
                    {
                        skipCounter = 0;
                        break;
                    }

                    if(skipCounter == weeksToSkip)
                    {
                        generatedDates.Add(new DateItem {
                            Date = recurringDate.Date,
                            DateStart = recurringDate.Date.Add(timeStart),
                            DateEnd = recurringDate.Date.Add(timeStart).Add(timeDuration)
                        });
                        skipCounter = 0;
                        break;
                    }

                    recurringDate = recurringDate.AddDays(1);
                }
                while (skipCounter != weeksToSkip);

                recurringDate = recurringDate.FirstDayOfMonth().AddMonths(monthsToSkip);
            }
        }

        return generatedDates;
    }

    public IEnumerable<DateItem> GenerateNoRepeat(DateTime scheduleFrom, DateTime scheduleTo)
    {
        IList<DateItem> generatedDates = new List<DateItem>();

        generatedDates.Add(new DateItem {
            Date = scheduleFrom.Date,
            DateStart = scheduleFrom,
            DateEnd = scheduleTo
        });

        return generatedDates;
    }
    
    public record DateItem 
    {
        public DateTime Date {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
    }
}