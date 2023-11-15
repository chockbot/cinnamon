using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;

public class VerifyCallbackArgs : IInteractor
{
    public string TransactionId {get; set;}
    public string CallbackToken {get; set;}
    public string Status {get; set;}
    public object Payload {get; set;}
}