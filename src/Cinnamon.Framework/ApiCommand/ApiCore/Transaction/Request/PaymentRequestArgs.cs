using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class PaymentRequestArgs 
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public DateTime SelectedDate {get; set;}

    [Required]
    public IEnumerable<RequestPaymentTicket> SelectedTickets {get; set;} = Enumerable.Empty<RequestPaymentTicket>();

    public string? Guid {get; set;}

    public string? Token {get; set;}

    public IEnumerable<ProviderQuestion>? Questions {get; set;}

    public class RequestPaymentTicket 
    {
        [Required]
        public int TicketId {get; set;}

        [Required]
        public int TicketCount {get; set;}
        
        public string SeatNumber { get; set; }

        public string CategoryUUID { get; set; }

        public string RowUUID { get; set; }

        public string SeatUUID { get; set; }
    }

    public class ProviderQuestion 
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Question {get; set;}

        public string? Answer {get; set;}
    }
}