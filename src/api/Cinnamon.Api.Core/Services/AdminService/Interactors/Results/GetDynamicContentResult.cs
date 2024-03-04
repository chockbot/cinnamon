namespace Cinnamon.Api.Core.Services.AdminService.Interactors.Results;

public class GetDynamicContentResult
{
    public int Id {get; set;}
    public string Identifier {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Content {get; set;}
    public DateTime DateLastUpdated {get; set;}
}