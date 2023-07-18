using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class IsAccountBlockedArgs : IInteractor
{
    public string Email { get; set; }
}
