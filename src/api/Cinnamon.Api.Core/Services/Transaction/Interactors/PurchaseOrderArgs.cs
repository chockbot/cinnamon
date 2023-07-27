using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class PurchaseOrderArgs : IInteractor
{
    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}
    public int NumberOfHeads {get; set;}
    public string? CouponCode {get; set;}
    public string PaymentMethod {get; set;}
    public string? PaymentChannel {get; set;}
    public IEnumerable<Enrollee> Students {get; set;}
    public CardDetails? CardInformation {get; set;}
    public bool IsCreditsApplied {get; set;}
    public string SelectedPeriod { get; set; }

    public class Enrollee
    {
        public int FamilyMemberId {get; set;}
        public string Name {get; set;}
    }

    public class CardDetails 
    {
        public string CardNumber {get; set;}
        public string AccountHolder {get; set;}
        public string CVV {get; set;}
        public string ExpireMonthYear {get; set;}
    }
}