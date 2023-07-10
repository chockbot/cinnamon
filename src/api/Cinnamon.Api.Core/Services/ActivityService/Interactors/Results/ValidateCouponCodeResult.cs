namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class ValidateCouponCodeResult 
{
    public bool IsValid {get; set;}
    public decimal MaximumSpend {get; set;}
    public int DiscountType {get; set;}
    public decimal Amount {get; set;}
}