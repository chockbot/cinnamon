using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class PaymentRequestArgs 
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public string SelectedDate {get; set;} // format yyyyMMddHHmmss

    [Required]
    public IEnumerable<RequestPaymentTicket> SelectedTickets {get; set;} = Enumerable.Empty<RequestPaymentTicket>();

    public string? Guid {get; set;}

    public string? Token {get; set;}

    public class RequestPaymentTicket 
    {
        [Required]
        public int TicketId {get; set;}

        [Required]
        public int TicketCount {get; set;}
    }
}