using Cinnamon.Framework.Interactor;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class SaveEmailTemplateArgs : IInteractor
{
    public int ActivityId {get; set;}
    public int ProviderId {get; set;}
    public EmailTemplateType TemplateType {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
}