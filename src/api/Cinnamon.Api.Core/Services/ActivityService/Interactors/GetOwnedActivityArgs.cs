using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GetOwnedActivityArgs : IInteractor
{
    public int ActivityId {get; set;}
    public bool IncludeAtivitySchedules {get; set;}
    public bool IncludeActivityAddress {get; set;}
    public bool IncludeActivityDescription {get; set;}
    public bool IncludeActivitySearchTags {get; set;}
    public bool IncludeActivityImages {get; set;}
    public bool? IsActive {get; set;}
    public bool? IncludeCustomer {get; set;}
    public bool? IncludeStudents { get; set; }
}