// ICreateGuestCustomerHandler.cs
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

namespace Cinnamon.Api.Core.Services.AccountService.Handlers
{
    public interface ICreateGuestCustomerHandler : IInteractorHandler<CreateGuestCustomerArgs, AppResult<CreateGuestCustomerResult>>
    {
    }
}