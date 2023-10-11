namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

public class OtePurchaseOrderDetailsResult 
{
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public IEnumerable<Ticket> Tickets {get; set;}
    public string PaymentMethod {get; set;}
    public decimal SubTotal {get; set;}
    public decimal ServiceFee {get; set;}
    public decimal HandlingFee {get; set;}
    public decimal TotalPurchase {get; set;}

    public class Ticket 
    {
        public string TicketName {get; set;}
        public decimal Price {get; set;}
        public int Count {get; set;}
    }
}