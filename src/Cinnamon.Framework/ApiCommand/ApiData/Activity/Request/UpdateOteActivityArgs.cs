using System.ComponentModel.DataAnnotations;
using static Cinnamon.Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class UpdateOteActivityArgs 
{

    [Required]
    public UpdateOteActivity Activity {get; set;}

    [Required]
    public IList<UpdateOtePricing> Pricings {get; set;}

    [Required]
    public IList<UpdateOteOnlineEvent> OnlineEvents { get; set; }

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
        [Required]
        public int Id { get; set; }

        [Required]
        public string VideoLink { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string TicketRestriction { get; set; }

        [Required]
        public int OteSchedulePricingGroupId { get; set; }
    }
}