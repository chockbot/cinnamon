using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AdminService.Handlers
{
    public interface IGetAdminUserByEmailHandler : IInteractorHandler<GetAdminUserByEmailArgs, AppResult<GetAdminUserByEmailResult>>
    {
    }
}
