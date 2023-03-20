using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;

public class VerifyPayoutCallbackArgs : IInteractor
{
    public string ReferenceId {get; set;}
    public string CallbackToken {get; set;}
    public string Status {get; set;}
    public string? FailureCode {get; set;}
}