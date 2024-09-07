using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class OteCreateRequestPaymentArgs : IInteractor
{
    public int CustomerId {get; set;}
    public int ActivityId {get; set;}
    public DateTime SelectedDate {get; set;}
    public IEnumerable<RequestPaymentTicket> SelectedTickets {get; set;} = Enumerable.Empty<RequestPaymentTicket>();
    public string? Guid {get; set;}
    public string? Token {get; set;}

    public bool Waitlisted {get; set;}
    public int WaitListId {get; set;}

    public IEnumerable<ProviderQuestion>? Questions {get; set;}

    public bool ForceCreateTicket {get; set;}

    public bool Used {get; set;}

    public class RequestPaymentTicket 
    {
        public int TicketId {get; set;}
        public int TicketCount {get; set;}
        public string SeatNumber { get; set; }
        public string CategoryUUID { get; set; }
        public string RowUUID { get; set; }
        public string SeatUUID { get; set; }
    }

    public class ProviderQuestion 
    {
        public int Id {get; set;}
        public string Question {get; set;}
        public string? Answer {get; set;}
    }
}