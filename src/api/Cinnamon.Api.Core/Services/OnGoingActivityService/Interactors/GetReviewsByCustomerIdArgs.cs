using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class GetReviewsByCustomerIdArgs : IInteractor
{
    public int CustomerId { get; set; }
}
