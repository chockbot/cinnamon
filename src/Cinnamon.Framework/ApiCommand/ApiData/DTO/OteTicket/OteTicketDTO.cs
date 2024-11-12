using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteTicket;

public class OteTicketDTO 
{
    public int Id {get; set;}
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
    public string Payload { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public int OteDateId {get; set;}
    public string SeatNumber { get; set; }
    public CustomerDTO Customer {get; set;}
    public PurchaseOrderDTO PurchaseOrder { get; set; }
}