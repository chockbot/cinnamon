using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class OteFinishTransactionArgs : IInteractor 
{
    public int TransactionId {get; set;}
}