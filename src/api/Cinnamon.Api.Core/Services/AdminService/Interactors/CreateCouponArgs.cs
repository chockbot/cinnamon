using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Interactors;

public class CreateCouponArgs : IInteractor
{
    public int ActivityId {get; set;}
    public string Name {get; set;}
    public string Code {get; set;}
    public int DiscountType {get; set;}
    public decimal Amount {get; set;}
    public decimal MaximumSpend {get; set;}
    public DateTime FromDate {get; set;}
    public DateTime ToDate {get; set;}
}