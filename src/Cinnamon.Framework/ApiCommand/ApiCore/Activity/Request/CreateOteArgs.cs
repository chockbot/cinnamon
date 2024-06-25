using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class CreateOteArgs
{
    [Required]
    public OteActivity Activity {get; set;}

    [Required]
    public IEnumerable<OtePricing> Pricings {get; set;}

    public IEnumerable<OteOnlineEvent>? OnlineEvents { get; set; }

    public IEnumerable<DateOverride>? DateOverrides {get; set;}

    public class OteActivity 
    {
        [Required]
        public string EventName {get; set;}

        [Required]
        public string Description {get; set;}

        [Required]
        public int ExperienceTypeId {get; set;}

        [Required]
        public int CategoryId {get; set;}

        public string? HouseNo {get; set;}

        public string? BarangayCode {get; set;}

        public string? BarangayName {get; set;}

        public string? CityNumber {get; set;}

        public string? CityName {get; set;}

        public string? RegionCode {get; set;}

        public string? RegionName {get; set;}

        public string? PostalCode {get; set;}

        public string? PinnedLocation {get; set;}

        [Required]
        public bool IsPublished {get; set;}

        [Required]
        public int ExperienceCreationTypeId {get; set;}

        [Required]
        public DateTime ScheduleFrom {get; set;}

        [Required]
        public DateTime ScheduleTo {get; set;}

        [Required]
        public string Recurrence {get; set;}

        [Required]
        public bool IsComingSoon {get; set;}


        // below fields are for recurrence options
        public DateTime? DurationStart {get; set;}
        public DateTime? DurationEnd {get; set;}
        public int? DurationEvery {get; set;}

        // week option field
        public string? WeekString {get; set;}

        // month option fields
        public int? MonthSelection {get; set;}
        public string? MonthRepeat {get; set;}
        public string? MonthDay {get; set;}
        public int? OnDayDate {get; set;}

        [Range(1, int.MaxValue)]
        public int EventDurationCount {get; set;}

        [Required]
        public string EventDurationTimeUnit {get; set;}
        public int EventTicketLimit { get; set; }
        public bool IsOpen { get; set; }
    }

    public class OtePricing 
    {
        [Required]
        public string Name {get; set;}
        
        [Required]
        public string Description {get; set;}

        [Required]
        public bool IsAbsorbFees {get; set;}

        [Required]
        public int MaxSlots {get; set;}

        [Required]
        public decimal Price {get; set;}
    }

    public class OteOnlineEvent
    {
        public string VideoLink { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TicketRestriction { get; set; }

        public int OteSchedulePricingGroupId { get; set; }
    }

    public class DateOverride 
    {
        [Required]
        public DateTime Date {get; set;}

        [Required]
        public TimeSpan TimeStart {get; set;}

        [Required]
        public TimeSpan TimeEnd {get; set;}
    }
}