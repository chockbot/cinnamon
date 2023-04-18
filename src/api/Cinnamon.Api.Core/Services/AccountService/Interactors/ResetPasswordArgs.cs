using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class ResetPasswordArgs: IInteractor
{
    public string Email {get; set;}
    public string ValidationRoute {get; set;}
}
