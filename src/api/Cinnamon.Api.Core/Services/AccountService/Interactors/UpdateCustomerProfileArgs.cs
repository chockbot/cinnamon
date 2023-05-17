using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UpdateCustomerProfileArgs : IInteractor
{
    public bool? IsOG { get; set; }
    public DateTime? IsOGDate { get; set; }
    public bool? IsOF { get; set; }
    public DateTime? IsOFDate { get; set; }
    public int? VerifiedBadge { get; set; }
    public DateTime? IsVerifiedDate { get; set; }
    public int CustomerId { get; set; }
}