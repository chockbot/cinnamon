using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class RequestPaymentArgs : IInteractor 
{
    public decimal Amount {get; set;}
    public string AmountCurrency {get; set;}
    public int TransactionId {get; set;}
    public int CustomerId {get; set;}
    public string PaymentMethod {get; set;}
    public string PaymentChannel {get; set;}
    public IEnumerable<MetaData> MetaDatas {get; set;}
    public CardDetails? CardInformation {get; set;}
    public string SuccessUrl {get; set;}
    public string FailedUrl {get; set;}

    public class CardDetails 
    {
        public string CardNumber {get; set;}
        public string AccountHolder {get; set;}
        public string CVV {get; set;}
        public string ExpireMonthYear {get; set;}
    }
}