using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteUpdateArgs : IInteractor
{
    public OteActivity Activity {get; set;}

    public IEnumerable<OtePricing> Pricings {get; set;}

    public IEnumerable<OteOnlineEvent>? OnlineEvents { get; set; }

    public IEnumerable<DateOverride>? DateOverrides { get; set; }

    public IEnumerable<OteReschedule>? OteReschedules {get; set;}

    public class OteActivity 
    {
        public int Id {get; set;}

        public string EventName {get; set;}

        public string Description {get; set;}

        public int ExperienceTypeId {get; set;}

        public int CategoryId {get; set;}

        public string HouseNo {get; set;}

        public string BarangayCode {get; set;}

        public string BarangayName {get; set;}

        public string CityNumber {get; set;}

        public string CityName {get; set;}

        public string RegionCode {get; set;}

        public string RegionName {get; set;}

        public string PostalCode {get; set;}

        public string PinnedLocation {get; set;}

        public bool IsPublished {get; set;}

        public DateTime ScheduleFrom {get; set;}

        public DateTime ScheduleTo {get; set;}

        public string Recurrence {get; set;}
        public bool IsComingSoon {get; set;}

        public DateTime? DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
        public int? DurationEvery { get; set; }

        // week option field
        public string? WeekString { get; set; }

        // month option fields
        public int? MonthSelection { get; set; }
        public string? MonthRepeat { get; set; }
        public string? MonthDay { get; set; }
        public int? OnDayDate { get; set; }

        public int EventDurationCount { get; set; }
        public string EventDurationTimeUnit { get; set; }
        public int EventTicketLimit { get; set; }
        public bool IsOpen { get; set; }
        public bool IsCapacity { get; set; }
        public int CapacityCount { get; set; }

        public int EmailFeedbackDays {get; set;}
        public int EmailReminderDays {get; set;}

        public string? FeedbackSubject {get; set;}
        public string? FeedbackBody {get; set;}
        public string? ReminderSubject {get; set;}
        public string? ReminderBody {get; set;}
    }

    public class OtePricing 
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public string Description {get; set;}

        public bool IsAbsorbFees {get; set;}

        public int MaxSlots {get; set;}

        public decimal Price {get; set;}
    }
    public class OteOnlineEvent
    {
        public int Id { get; set; }

        public string VideoLink { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TicketRestriction { get; set; }
    }
    public class DateOverride
    {
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
    }

    public class OteReschedule 
    {
        public int Id {get; set;}
        public DateTime OldDate {get; set;}
        public DateTime NewDate {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
    }
}