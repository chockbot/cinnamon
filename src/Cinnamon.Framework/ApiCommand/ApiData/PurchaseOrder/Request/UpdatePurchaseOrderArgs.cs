using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class UpdatePurchaseOrderArgs 
{
    [Required]
    public int PurchaseOrderId {get; set;}
    public int? ScheduleId {get; set;}
    public decimal? Total {get; set;}
    public decimal? ConvinienceFee {get; set;}
    public string? Coupon {get; set;}
    public decimal? CouponAmount {get; set;}
    public decimal? OverallTotal {get; set;}
}