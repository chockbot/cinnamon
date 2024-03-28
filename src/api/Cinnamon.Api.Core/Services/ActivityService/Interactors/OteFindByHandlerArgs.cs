using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteFindByHandlerArgs : IInteractor
{
    public string Handler {get; set;}
    public bool IncludeAddress {get; set;}
    public bool IncludeDescription {get; set;}
    public bool IncludeSchedule {get; set;}
    public bool IncludePricing {get; set;}
    public bool IncludeImages {get; set;}
    public bool IncludeOnlineEvent { get; set; }
}