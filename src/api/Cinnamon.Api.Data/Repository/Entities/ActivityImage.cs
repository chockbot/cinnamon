namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityImage : BaseEntity 
{
    public int ActivityId {get; set;}
    public int Order {get; set;}
    public string ImageName {get; set;}
    public string ImageLocation {get; set;}

    public virtual Activity Activity {get; set;}
}