namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Coupon;

public class CouponDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public bool IsAdmin {get; set;}
    public int CustomerId {get; set;}
    public string Name {get; set;}
    public string Code {get; set;}
    public int DiscountType {get; set;}
    public decimal Amount {get; set;}
    public decimal MaximumSpend {get; set;}
    public DateTime FromDate {get; set;}
    public DateTime ToDate {get; set;}
    public int Status {get; set;}
}