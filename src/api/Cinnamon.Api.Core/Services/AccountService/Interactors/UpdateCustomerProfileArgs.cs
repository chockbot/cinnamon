using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UpdateCustomerProfileArgs : IInteractor
{
    public int? VerifiedBadge { get; set; }
    public int CustomerId { get; set; }
}