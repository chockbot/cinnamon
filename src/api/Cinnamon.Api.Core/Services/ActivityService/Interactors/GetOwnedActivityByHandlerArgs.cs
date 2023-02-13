using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GetOwnedActivityByHandlerArgs : IInteractor
{
    public string Handler {get; set;}
    public bool IncludeAtivitySchedules {get; set;}
    public bool IncludeActivityAddress {get; set;}
    public bool IncludeActivityDescription {get; set;}
    public bool IncludeActivitySearchTags {get; set;}
    public bool IncludeActivityImages {get; set;}
    public bool? IsActive {get; set;}
    public bool? IncludeCustomer {get; set;}
}