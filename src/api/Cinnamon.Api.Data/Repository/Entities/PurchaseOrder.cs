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
    public decimal CreditAmount {get; set;}
    // 0 = pending, 1 = succeed, 2 failed, 3 = cancelled/refunded, 5 = disbursement
    public int Status {get; set;}
    public string Payload {get; set;}

    public Activity Activity {get; set;}
    public ActivitySchedule Schedule {get; set;}
}