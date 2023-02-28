using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;
using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;
public interface IFailedLoginData
{
    Task<AppResult<CreateFailedLoginResult>> CreateFailedLogin(CreateFailedLoginArgs args);
    Task<AppResult<GetFailedLoginsResult>> GetFailedLogins(GetFailedLoginsArgs args);
    Task<AppResult<RemoveFailedLoginsResult>> RemoveFailedLogins(RemoveFailedLoginsArgs args);
}
