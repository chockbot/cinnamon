using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IAdminUserData
{
    Task<AppResult<GetAdminUserByEmailResult>> GetAdminUserByEmail(GetAdminUserByEmailArgs args);
}
