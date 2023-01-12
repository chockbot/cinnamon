using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitVerifyEmailArgs : IInteractor
{
    public string UserId {get; set;}
    public string Token {get; set;}
}