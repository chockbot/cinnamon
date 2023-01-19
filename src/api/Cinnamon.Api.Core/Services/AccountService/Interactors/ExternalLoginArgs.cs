using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class ExternalLoginArgs : IInteractor
{
    public string Email {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
}