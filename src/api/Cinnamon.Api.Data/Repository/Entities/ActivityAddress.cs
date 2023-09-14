namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityAddress : BaseEntity
{
    public int ActivityId {get; set;}
    public string Address1 {get; set;} = string.Empty;
    public string Address2 {get; set;} = string.Empty;
    public string District {get; set;} = string.Empty;
    public string City {get; set;} = string.Empty;
    public string CityName {get; set; } = string.Empty;
    public string Subdivision { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public string Barangay { get; set; } = string.Empty;
    public string BarangayName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string PinnedLocation { get; set; } = string.Empty;
    public virtual Activity Activity {get; set;}
}