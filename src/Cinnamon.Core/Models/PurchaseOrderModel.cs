namespace Cinnamon.Core.Models;

public class PurchaseOrderModel : BaseModel
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public decimal Total {get; set;}
    public decimal ConvinienceFee {get; set;}
    public string? Coupon {get; set;}
    public decimal? CouponAmount {get; set;}
    public decimal OverallTotal {get; set;}
}