using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class GetRequestRefundArgs: IInteractor
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId { get; set; }
    public bool? IncludeCustomer { get; set; }
    public bool? IncludePurchaseOrder { get; set; }
    public int? Status { get; set; }
    public bool? IsAdmin { get; set;}
}
