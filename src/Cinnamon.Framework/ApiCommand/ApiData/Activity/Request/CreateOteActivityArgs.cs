using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class CreateOteActivityArgs 
{

    [Required]
    public OteActivity Activity {get; set;}

    [Required]
    public IList<OtePricing> Pricings {get; set;}

    public IList<OteOnlineEvent>? OnlineEvents { get; set; }

    [Required]
    public IList<OteDate> Dates {get; set;}
    
    public IList<OteDateOverride>? DateOverrides {get; set;}


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

        [Required]
        public int CustomerId {get; set;}

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
        [Range(1,3)]
        public int ExperienceCreationTypeId {get; set;}

        [Required]
        public DateTime ScheduleFrom {get; set;}

        [Required]
        public DateTime ScheduleTo {get; set;}

        [Required]
        public string Recurrence {get; set;}

        [Required]
        public bool IsComingSoon {get; set;}

        public string? ExtraOptions {get; set;} = string.Empty;

        [Required]
        public DateTime RecurrenceDateStart {get; set;}

        [Required]
        public DateTime RecurrenceDateEnd {get; set;}

        [Required]
        public int RepeatEvery {get; set;}

        public string SelectedDays {get; set;} = string.Empty;

        [Required]
        public int EventDurationCount {get; set;}

        [Required]
        public string EventDurationTimeUnit {get; set;}

        [Required]
        public int EventTicketLimit { get; set; }

        public bool IsOpen { get; set; }

        public bool IsCapacity { get; set; }
        public int CapacityCount { get; set; }

        [Required]
        public int EmailFeedbackDays {get; set;}

        [Required]
        public int EmailReminderDays {get; set;}
    }

    public class OtePricing 
    {
        [Required]
        public string Name {get; set;}
        
        public string Description {get; set;}

        [Required]
        public bool IsAbsorbFees {get; set;}

        public int MaxSlots {get; set;}

        [Required]
        public decimal Price {get; set;}

        [Required]
        public bool RequiredApproval {get; set;}
        [Required]
        public bool IsUnlimited { get; set; }
    }

    public class OteOnlineEvent
    {
        public string VideoLink { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TicketRestriction { get; set; }

        public int OteSchedulePricingGroupId { get; set; }
    }

    public class OteDate 
    {
        [Required]
        public DateTime Date {get; set;}

        [Required]
        public DateTime DateStart {get; set;}

        [Required]
        public DateTime DateEnd {get; set;}
    }

    public class OteDateOverride 
    {
        [Required]
        public DateTime Date {get; set;}

        [Required]
        public DateTime DateStart {get; set;}

        [Required]
        public DateTime DateEnd {get; set;}
    }
}