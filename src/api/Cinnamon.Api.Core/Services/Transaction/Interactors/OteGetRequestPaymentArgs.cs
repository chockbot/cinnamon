using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class OteGetRequestPaymentArgs : IInteractor
{
    public string Guid {get; set;}
    public string Token {get; set;}
}