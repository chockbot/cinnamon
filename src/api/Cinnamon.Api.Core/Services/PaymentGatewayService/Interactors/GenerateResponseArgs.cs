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
}