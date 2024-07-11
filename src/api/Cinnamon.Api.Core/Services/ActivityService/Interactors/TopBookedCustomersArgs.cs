using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class TopBookedCustomersArgs : IInteractor
{
    public int ActivityId {get; set;}

    public DateTime BookedDate {get; set;}
}