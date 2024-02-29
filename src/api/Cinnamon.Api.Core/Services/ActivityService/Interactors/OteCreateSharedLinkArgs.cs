using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteCreateSharedLinkArgs : IInteractor
{
    public int ActivityId {get; set;}
    public int OteDateId {get; set;}
}