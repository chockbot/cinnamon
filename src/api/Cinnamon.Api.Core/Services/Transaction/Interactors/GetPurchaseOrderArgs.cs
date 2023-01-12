using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class GetPurchaseOrderArgs : IInteractor
{
    public int PurchaseOrderId {get; set;}
}