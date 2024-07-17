using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class ApprovedFreeWaitListArgs : IInteractor 
{
    public int WaitListId {get; set;}
}