namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class EmailTemplateResult
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
}