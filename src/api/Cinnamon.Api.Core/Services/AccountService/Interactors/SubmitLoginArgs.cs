using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitLoginArgs : IInteractor
{
    public string Email {get; set;}
    public string Password {get; set;}
}