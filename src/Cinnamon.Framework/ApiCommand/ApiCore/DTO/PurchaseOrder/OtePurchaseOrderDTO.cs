namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.PurchaseOrder;

public class OtePurchaseOrderDTO 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public IEnumerable<PurchaseTicket> Tickets {get; set;}
    public DateTime PurchasedDate {get; set;}
    public string PaymentMethod {get; set;}
    public decimal SubTotal {get; set;}
    public decimal ServiceFee {get; set;}
    public decimal HandlingFee {get; set;}
    public decimal TotalPurchase {get; set;}
    public string TicketUrl {get; set;}
    public decimal? Discount {get; set;}

    public class PurchaseTicket 
    {
        public int Id {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
        public string ImageData {get; set;}
        public string SeatNumber { get; set; }
    }
}