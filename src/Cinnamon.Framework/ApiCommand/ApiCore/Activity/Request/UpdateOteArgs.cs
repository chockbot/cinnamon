using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class UpdateOteArgs
{
    [Required]
    public OteActivity Activity {get; set;}

    [Required]
    public IEnumerable<OtePricing> Pricings {get; set;}

    public class OteActivity 
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
    }

    public class OtePricing 
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Description {get; set;}

        [Required]
        public bool IsAbsorbFees {get; set;}

        [Required]
        public int MaxSlots {get; set;}

        [Required]
        public decimal Price {get; set;}
    }

}