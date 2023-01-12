using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitResendVerificationArgs : IInteractor
{
    public string Email {get; set;}
    public string ValidationRoute {get; set;}
}