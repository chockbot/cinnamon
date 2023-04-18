using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class RequestRefundArgs: IInteractor
{
    public int PurchaseOrderId {get; set;}
    public string Reason {get; set;}
}
