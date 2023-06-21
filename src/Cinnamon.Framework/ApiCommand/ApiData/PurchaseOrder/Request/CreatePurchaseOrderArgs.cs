using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class CreatePurchaseOrderArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public int ScheduleId {get; set;}
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public decimal Total {get; set;}
    [Required]
    public decimal ConvinienceFee {get; set;}
    public string? Coupon {get; set;} = string.Empty;
    [Required]
    public decimal CouponAmount {get; set;}
    [Required]
    public decimal OverallTotal {get; set;}
    [Required]
    [Range(0,2)]
    public int Status {get; set;}
    public string? Payload {get; set;}
    public decimal CreditAmount {get; set;}
    [Required]
    public decimal UnitPrice {get; set;}
    [Required]
    public int UnitCount {get; set;}
}