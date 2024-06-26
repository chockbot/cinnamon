using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GetOteWaitlistByProviderArgs : IInteractor
{
    public int ProviderId { get; set; }
    public int ActivityId { get; set; }
}
