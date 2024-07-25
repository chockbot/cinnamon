namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

public class OteCreateRequestPaymentResult 
{
    public int ActivityId {get; set;}
    public DateTime SelectedDate {get; set;}
    public IEnumerable<RequestPaymentTicket> SelectedTickets {get; set;} = Enumerable.Empty<RequestPaymentTicket>();
    public string Guid {get; set;}
    public string Token {get; set;}

    public class RequestPaymentTicket 
    {
        public int TicketId {get; set;}
        public int TicketCount {get; set;}
    }
}