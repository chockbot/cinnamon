namespace Cinnamon.Api.Data.Repository.Entities;

public class Coupon : BaseEntity 
{
    // if 0 or null then apply to all else apply only in associated activity
    public int? ActivityId {get; set;}
    public bool IsAdmin {get; set;}
    public int CustomerId {get; set;}
    public string Name {get; set;}
    public string Code {get; set;}
    // 0 = percentage, 1 = fixed amount
    public int DiscountType {get; set;}
    // for percentage or fixed amount
    public decimal Amount {get; set;}
    public decimal MaximumSpend {get; set;}
    public DateTime From {get; set;}
    public DateTime To {get; set;}
    public int Status {get; set;}

    public virtual Activity Activity {get; set;}
    public virtual Customer Customer {get; set;}
}