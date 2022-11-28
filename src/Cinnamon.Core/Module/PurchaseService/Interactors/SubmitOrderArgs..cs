using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.PurchaseService.Interactors;

public class SubmitOrderArgs : IInteractor 
{
    public int ActivityId {get; set;}
    public int CustomerId {get; set;}
    public int ScheduleId {get; set;}
    public int NumberOfHeads {get; set;}
    public string? CouponCode {get; set;}
}