namespace Cinnamon.Web.Models.Entities;

public class PurchaseOrder 
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public string ActivityName {get; set;}
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
    public string ExperienceBy {get; set;}
    public string ExperienceByContactEmail {get; set;}
    public string ExperienceByContactNo {get; set;}
    public decimal AppliedCreditAmount {get; set;}
    public decimal AddOnsAmount { get; set; }
    public IList<AddOnDetail> AddOnDetails { get; set;}
    public string Payload { get; set; }
    public class AddOnDetail
    {
        public int Id { get; set;}  
        public string Name { get; set;}
        public int Count { get; set; }
    }
}