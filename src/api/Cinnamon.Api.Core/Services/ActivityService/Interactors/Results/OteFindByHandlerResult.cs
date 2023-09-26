namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class OteFindByHandlerResult 
{
    public int Id {get; set;}
    public string EventName {get; set;}
    public string Description {get; set;}
    public int ExperienceTypeId {get; set;}
    public int CategoryId {get; set;}
    public string Price {get; set;}
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
    public string Handler {get; set;}
    public DateTime ScheduleFrom {get; set;}
    public DateTime ScheduleTo {get; set;}
    public string Recurrences {get; set;}
    public int ProviderId {get; set;}
    public IEnumerable<OtePricing> Pricings {get; set;}
    public IEnumerable<Image> Images {get; set;}
    public bool IsComingSoon {get; set;}

    public class OtePricing 
    {
        public int Id {get; set;}
        public int OteScheduleId { get; set; }
        public decimal Price { get; set; }
        public int MaxSlots { get; set; }
        public string Description { get; set; }
        public bool IsAbsorbFees { get; set; }
    }

    public class Image 
    {
         public int Id { get; set; }
        public int Order {get; set;}
        public int ActivityId {get; set;}
        public string ImageName { get; set; }
        public string ImageLocation { get; set; }
    }
}