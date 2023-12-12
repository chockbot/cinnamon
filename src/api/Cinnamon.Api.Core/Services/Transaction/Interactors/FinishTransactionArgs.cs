using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class FinishTransactionArgs : IInteractor 
{
    public int TransactionId {get; set;}

    public decimal AddOnsAmount { get; set; }
}