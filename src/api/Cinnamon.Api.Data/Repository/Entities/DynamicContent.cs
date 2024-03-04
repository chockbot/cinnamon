namespace Cinnamon.Api.Data.Repository.Entities;

public class DynamicContent : BaseEntity 
{
    public string Identifier {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Content {get; set;}
    public DateTime DateLastUpdated {get; set;}
}