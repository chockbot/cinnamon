using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class PurchaseOrderArgs : IInteractor
{
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int NumberOfHeads {get; set;}
    public string? CouponCode {get; set;}
}