using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class UpdateCreditBalanceArgs : IInteractor 
{
    public int CustomerId {get; set;}
    public decimal Amount {get; set;}
    // 0 = add credit, 1 = subtract credit
    public int ActionFlag {get; set;}
}