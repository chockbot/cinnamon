namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityAddress : BaseEntity
{
    public int ActivityId {get; set;}
    public string Address1 {get; set;}
    public string Address2 {get; set;}
    public string District {get; set;}
    public string City {get; set;}
    public string Subdivision { get; set; }
    public string Region { get; set; }
    public string Barangay { get; set; }
    public string PostalCode { get; set; }
    public virtual Activity Activity {get; set;}
}