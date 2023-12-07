using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;

public class GenerateResponseArgs : IInteractor
{
    public string PaymentChannel {get; set;}
    public int TransactionId {get; set;}
    public decimal Amount {get; set;}
    public string AmountCurrency {get; set;}
    public IEnumerable<MetaData> MetaDatas {get; set;}
    // for card payment
    public CardInformation? CardDetails {get; set;}
    public string SuccessUrl {get; set;}
    public string FailedUrl {get; set;}

    public class CardInformation 
    {
        public string CardNumber {get; set;}
        public int ExpiryMonth {get; set;}
        public int ExpiryYear {get; set;}
        public string Cvv {get; set;}
        public string CardHolderName {get; set;}
    }
}