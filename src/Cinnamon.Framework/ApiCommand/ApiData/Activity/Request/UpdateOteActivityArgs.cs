using System.ComponentModel.DataAnnotations;
using static Cinnamon.Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class UpdateOteActivityArgs 
{

    [Required]
    public UpdateOteActivity Activity {get; set;}

    [Required]
    public IList<UpdateOtePricing> Pricings {get; set;}

    public IList<UpdateOteDate> Dates { get; set; }

    public IList<UpdateOteOnlineEvent>? OnlineEvents { get; set; }

    public IList<UpdateOteDateOverride>? DateOverrides { get; set; }

    [Required]
    public bool RecreateSchedule {get; set;}

    public IList<OteReschedule>? OteReschedules {get; set;}

    public class UpdateOteActivity 
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

        [Required]
        public string StringPrice {get; set;}

        public string? HouseNo {get; set;} = string.Empty;

        public string? BarangayCode {get; set;} = string.Empty;

        public string? BarangayName {get; set;} = string.Empty;

        public string? CityNumber {get; set;} = string.Empty;

        public string? CityName {get; set;} = string.Empty;

        public string? RegionCode {get; set;} = string.Empty;

        public string? RegionName {get; set;} = string.Empty;

        public string? PostalCode {get; set;} = string.Empty;

        public string? PinnedLocation {get; set;} = string.Empty;

        [Required]
        public bool IsPublished {get; set;}

        [Required]
        public string Handler {get; set;}

        [Required]
        public DateTime ScheduleFrom {get; set;}

        [Required]
        public DateTime ScheduleTo {get; set;}

        [Required]
        public string Recurrence {get; set;}

        [Required]
        public bool IsComingSoon {get; set;}

        public string? ExtraOptions { get; set; } = string.Empty;

        [Required]
        public DateTime RecurrenceDateStart { get; set; }

        [Required]
        public DateTime RecurrenceDateEnd { get; set; }

        [Required]
        public int RepeatEvery { get; set; }

        public string SelectedDays { get; set; } = string.Empty;

        [Required]
        public int EventDurationCount { get; set; }

        [Required]
        public string EventDurationTimeUnit { get; set; }

        [Required]
        public int EventTicketLimit { get; set; }
    }

    public class UpdateOtePricing 
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
        [Range(1, int.MaxValue)]
        public int MaxSlots {get; set;}

        [Required]
        public decimal Price {get; set;}
    }

    public class UpdateOteOnlineEvent
    {
        public int Id { get; set; }
        public string VideoLink { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TicketRestriction { get; set; }

        public int OteSchedulePricingGroupId { get; set; }
    }
    public class UpdateOteDate
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }
    }
    public class UpdateOteDateOverride
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }
    }

    public class OteReschedule 
    {
        [Required]
        public int Id {get; set;}
        
        [Required]
        public DateTime OldDate {get; set;}

        [Required]
        public DateTime NewDate {get; set;}

        [Required]
        public DateTime DateStart {get; set;}

        [Required]
        public DateTime DateEnd {get; set;}
    }
}