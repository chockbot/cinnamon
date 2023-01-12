namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityAddress : BaseEntity
{
    public int ActivityId {get; set;}
    public string Address1 {get; set;}
    public string Address2 {get; set;}
    public string District {get; set;}
    public string City {get; set;}

    public virtual Activity Activity {get; set;}
}