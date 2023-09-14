using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class CreateOteActivityArgs 
{

    [Required]
    public OteActivity Activity {get; set;}

    [Required]
    public IList<OtePricing> Pricings {get; set;}

    public class OteActivity 
    {
        [Required]
        public string EventName {get; set;}

        [Required]
        public string Description {get; set;}

        [Required]
        public int ExperienceTypeId {get; set;}

        [Required]
        public int CustomerId {get; set;}

        [Required]
        public string StringPrice {get; set;}

        [Required]
        public string HouseNo {get; set;}

        [Required]
        public string BarangayCode {get; set;}

        [Required]
        public string BarangayName {get; set;}

        [Required]
        public string CityNumber {get; set;}

        [Required]
        public string CityName {get; set;}

        [Required]
        public string RegionCode {get; set;}

        [Required]
        public string RegionName {get; set;}

        [Required]
        public string PostalCode {get; set;}

        [Required]
        public string PinnedLocation {get; set;}

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
    }

    public class OtePricing 
    {
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
}