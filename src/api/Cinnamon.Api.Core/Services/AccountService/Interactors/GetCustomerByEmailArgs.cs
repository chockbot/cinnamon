using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class GetCustomerByEmailArgs: IInteractor
{
    public string Email { get; set; }
}
