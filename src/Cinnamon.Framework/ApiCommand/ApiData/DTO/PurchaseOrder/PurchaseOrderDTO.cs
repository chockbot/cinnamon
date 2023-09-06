namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;

public class PurchaseOrderDTO 
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int CustomerId {get; set;}
    public decimal Total {get; set;}
    public DateTime PurchaseDate {get; set;}
    public decimal ConvinienceFee {get; set;}
    public string? Coupon {get; set;}
    public decimal? CouponAmount {get; set;}
    public decimal OverallTotal {get; set;}
    public int Status {get; set;}
    public string Payload {get; set;}
    public decimal CreditAmount {get; set;}
    public int UnitCount {get; set;}
    public decimal UnitPrice {get; set;}
    public bool IsInclusivePayment {get; set;}
    public decimal PerUnitDisburseAmount {get; set;}
    public decimal TotalDisburseAmount {get; set;}

    public AssociatedActivity Activity {get; set;}
    public AssociatedSchedule Schedule {get; set;}

    public class AssociatedActivity 
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public int CreatedBy { get; set; }
    }

    public class AssociatedSchedule 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string DateTime {get; set;}
    }
}