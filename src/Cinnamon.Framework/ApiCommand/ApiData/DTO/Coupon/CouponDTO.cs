namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Coupon;

public class CouponDTO 
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int CustomerId {get; set;}
    public string Name {get; set;}
    public string Code {get; set;}
    public int DiscountType {get; set;}
    public decimal Amount {get; set;}
    public decimal MaximumSpend {get; set;}
    public DateTime From {get; set;}
    public DateTime To {get; set;}
    public int Status {get; set;}
    public bool IsAdmin {get; set;}
    public Activity? ActivityApplied {get; set;}

    public class Activity 
    {
        public int Id {get; set;}
        public string Title {get; set;}
    }
}