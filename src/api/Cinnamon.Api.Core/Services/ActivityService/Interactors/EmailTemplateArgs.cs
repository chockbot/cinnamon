using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class EmailTemplateArgs : IInteractor
{
    public int ActivityId {get; set;}
    public string TemplateType {get; set;}
}