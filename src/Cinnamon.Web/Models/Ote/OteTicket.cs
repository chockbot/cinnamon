using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Ote;
public class OteTicket
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int OteScheduleId { get; set; }
    public int OteSchedulePricingId { get; set; }
    public int CustomerId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string QRCode { get; set; }
    public string QRImageData { get; set; }
    public string Status { get; set; }
    public string Payload { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string Registration { get; set; }
    public string CustomerName { get; set; }
    public CustomerProfile Customer { get; set; }
}
