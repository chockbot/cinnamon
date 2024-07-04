using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class OtePurchaseOrderArgs : IInteractor 
{
    public int ActivityId {get; set;}
    public string? CouponCode {get; set;}
    public string PaymentMethod {get; set;}
    public string? PaymentChannel {get; set;}
    public CardDetails? CardInformation {get; set;}
    public bool IsCreditsApplied {get; set;}
    public IEnumerable<Ticket> Tickets {get; set;}

    public IEnumerable<ActivityQuestion>? Questions {get; set;}

    public class CardDetails 
    {
        public string CardNumber {get; set;}
        public string AccountHolder {get; set;}
        public string CVV {get; set;}
        public string ExpireMonthYear {get; set;}
    }

    public class Ticket 
    {
        public int Id {get; set;}
        public int Count {get; set;}
    }

    public class ActivityQuestion 
    {
        public int Id {get; set;}
        public string Question {get; set;}
        public string Answer {get; set;}
    }
}