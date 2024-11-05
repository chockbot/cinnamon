namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.PurchaseOrder;

public class PurchaseOrderDTO
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int CustomerId {get; set;}
    public decimal Total {get; set;}
    public decimal ConvinienceFee {get; set;}
    public string? Coupon {get; set;}
    public decimal? CouponAmount {get; set;}
    public decimal OverallTotal {get; set;}
    public int EnrolleeCount {get; set;}
    public string PaymentMethod {get; set;}
    public decimal ServiceFee {get; set;}
    public decimal PaymentProviderFee {get; set;}
    public decimal AppliedCredit {get; set;}
    public bool IsInclusivePayment {get; set;}
    public decimal AddOnsAmount { get; set; }
    public string Payload { get; set; }
    public IEnumerable<AddOnDetail> AddOnsDetails { get; set; }

    public class AddOnDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddOnCount { get; set; }
    }
}