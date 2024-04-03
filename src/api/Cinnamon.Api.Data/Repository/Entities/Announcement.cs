namespace Cinnamon.Api.Data.Repository.Entities;

public class Announcement : BaseEntity 
{
    public int AdminId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string ButtonLabel {get; set;}
    public string Link {get; set;}
    public string Status {get; set;}   // draft|published
}