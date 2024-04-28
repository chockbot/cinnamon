using System.ComponentModel.DataAnnotations;
using static Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request.CreateOteArgs;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UpdateOteArgs
{
    [Required]
    public OteUpdateActivity Activity {get; set;}

    [Required]
    public IEnumerable<OteUpdatePricing> Pricings {get; set;}

    public IEnumerable<DateOverride>? DateOverrides { get; set; }

    public IEnumerable<OteUpdateOnlineEvent>? OnlineEvents { get; set; }

    public IEnumerable<OteUpdateReschedule>? OteReschedules {get; set;}

    public class OteUpdateActivity 
    {
        [Required]
        public int Id {get; set;}

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
        public DateTime ScheduleFrom {get; set;}

        [Required]
        public DateTime ScheduleTo {get; set;}

        [Required]
        public string Recurrence {get; set;}

        [Required]
        public bool IsComingSoon {get; set;}
        public int? DurationEvery { get; set; }

        // week option field
        public string? WeekString { get; set; }

        // month option fields
        public int? MonthSelection { get; set; }
        public string? MonthRepeat { get; set; }
        public string? MonthDay { get; set; }
        public int? OnDayDate { get; set; }

        [Range(1, int.MaxValue)]
        public int EventDurationCount { get; set; }
        public string EventDurationTimeUnit { get; set; }

        public DateTime? DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
    }

    public class OteUpdatePricing 
    {
        [Required]
        public int Id {get; set;}

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
    public class OteUpdateOnlineEvent
    {
        public int Id { get; set; }
        public string VideoLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TicketRestriction { get; set; }
    }

    public class OteUpdateReschedule 
    {
        [Required]
        public DateTime OldDate {get; set;}

        [Required]
        public DateTime NewDate {get; set;}
    }
}