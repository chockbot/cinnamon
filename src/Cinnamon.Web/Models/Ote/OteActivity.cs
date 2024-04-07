using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Ote;

public class OteActivity 
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
    public bool IsComingSoon {get; set;}
    public int Slots { get; set; }
    public int Sold { get; set; }
    public int Available { get; set; }
    public string EventImage { get; set; }
    public bool ForceDisable {get; set;}
    public IEnumerable<OtePricing> Pricings {get; set;}
    public IEnumerable<ActivityImage> Images {get; set;}
    public IEnumerable<OteDate> OteDates {get; set;}

    // temp
    public int OteDateId {get; set;}
}