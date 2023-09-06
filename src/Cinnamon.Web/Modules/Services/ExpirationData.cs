using Cinnamon.Framework.Helpers;
using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Modules.Services;
public class ExpirationData
{
    public static DateTime CalculateFrequency(ActivitySchedule schedule)
    {
        DateTime frequency;
        if (schedule.SessionName.Contains("Weeks"))
        {
            int weeks = int.Parse(schedule.SessionName.Split()[0]);
            frequency = schedule.StartDate.Value.AddDays(7 * weeks);
            if (frequency <= DateTime.Now.Date)
            {
                var datePeriod = DateNextPeriod.CreateRecurring(schedule.StartDate.Value, schedule.SessionName);
                frequency = datePeriod.PeriodEnd.AddDays(1).Date.AddSeconds(-1);
            }
        }
        else
        {
            var datePeriod = DateNextPeriod.CreateRecurring(schedule.StartDate.Value, schedule.SessionName);
            frequency = datePeriod.PeriodEnd.AddDays(1).Date.AddSeconds(-1);
        }
        return frequency;
    }

    public static string GetOrdinal(int day)
    {
        if (day >= 11 && day <= 13)
        {
            return "th";
        }
        switch (day % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }
    public static string GetExpirationText(string sessionName, DateTime frequency, string ordinal)
    {
        return $"The expiration period is {sessionName.ToLower()} (calendar days), and is valid until the {frequency.Day}{GetOrdinal(frequency.Day)} day of {frequency.ToString("MMMM")}. Next period is available for purchase at checkout.";
    }   
}
