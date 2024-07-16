using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Interactors;

public class ApprovedPaidWaitListArgs : IInteractor 
{
    public int WaitListId {get; set;}
}