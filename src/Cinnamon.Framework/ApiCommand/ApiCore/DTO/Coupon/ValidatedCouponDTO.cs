namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Coupon;

public class ValidatedCouponDTO
{
    public bool IsValid {get; set;}
    public int DiscountType {get; set;}
    public decimal Amount {get; set;}
    public decimal MaximumSpend {get; set;}
}