namespace Cinnamon.Web.Models.Entities;

public class ActivityImage 
{
    public int Id {get; set;}
    public int Order {get; set;}
    public int ActivityId { get; set; }
    public string ImageSrc {get; set;}
    public string Name {get; set;}
}