using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class GetCustomerByIdArgs:IInteractor
{
    public int Id { get; set; }
}
