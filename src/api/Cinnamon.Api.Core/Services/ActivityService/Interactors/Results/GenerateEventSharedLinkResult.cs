namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GenerateEventSharedLinkResult
{
    public string GeneratedLink {get; set;}
    public bool Enable {get; set;}
    public string Guid {get; set;}
    public string Token {get; set;}
}