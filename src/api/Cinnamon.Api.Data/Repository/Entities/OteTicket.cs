namespace Cinnamon.Api.Data.Repository.Entities;

public class OteTicket : BaseEntity 
{
    public int ActivityId {get; set;}
    public int OteScheduleId {get; set;}
    public int OteSchedulePricingId {get; set;}
    public int CustomerId {get; set;}
    public int PurchaseOrderId {get; set;}
    public string Title {get; set;}
    public decimal Amount {get; set;}
    public string QRCode {get; set;}
    public string QRImageData {get; set;}
    public string Status {get; set;}
    public int? OteDateId {get; set;}
    public string SeatNumber { get; set; }
    public Activity Activity {get; set;}
    public OteSchedule OteSchedule {get; set;}
    public OteSchedulePricing OteSchedulePricing {get; set;}
    public Customer Customer {get; set;}
    public PurchaseOrder PurchaseOrder { get; set;}
    public OteDate? OteDate {get; set;}
}