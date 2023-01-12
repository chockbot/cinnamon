namespace Cinnamon.Api.Data.Repository.Entities;

public class PurchaseOrder : BaseEntity
{
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int CustomerId {get; set;}
    public decimal Total {get; set;}
    public decimal ConvinienceFee {get; set;}
    public string? Coupon {get; set;}
    public decimal? CouponAmount {get; set;}
    public decimal OverallTotal {get; set;}
}