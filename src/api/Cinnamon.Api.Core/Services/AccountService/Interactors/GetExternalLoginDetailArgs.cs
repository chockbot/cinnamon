using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class GetExternalLoginDetailArgs : IInteractor
{
    public string Token {get; set;}
    public string Guid {get; set;}
}