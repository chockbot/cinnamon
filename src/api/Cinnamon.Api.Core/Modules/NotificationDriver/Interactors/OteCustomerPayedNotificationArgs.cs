using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class OteCustomerPayedNotificationArgs : IInteractor 
{
    public string Email {get; set;}
    public string ProviderName {get; set;}
    public string ProviderEmail {get; set;}
    public string ProviderNumber {get; set;}
    public string CustomerName {get; set;}
    public string EventName {get; set;}
    public string EventLocation {get; set;}
    public DateTime EventDate {get; set;}
    public IEnumerable<TicketDetails> Tickets {get; set;}
    public string ReferenceNumber {get; set;}
    public string PaymentMethod {get; set;}
    public decimal SubTotal {get; set;}
    public decimal ServiceFee {get; set;}
    public decimal HandlingFee {get; set;}
    public decimal TotalAmount {get; set;}
    public string TicketDetailsLink {get; set;}
    public decimal? Discount {get; set;}

    public class TicketDetails 
    {
        public string TicketName {get; set;}
        public int TicketCount {get; set;}
        public decimal TicketPrice {get; set;}
        public string TicketSeatNumber { get; set; }
    }
}