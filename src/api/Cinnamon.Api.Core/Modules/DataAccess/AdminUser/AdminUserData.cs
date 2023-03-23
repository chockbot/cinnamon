using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AdminUser.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.AdminUser;

public class AdminUserData : IAdminUserData
{
    private readonly IFlurlClient flurlClient;
	public AdminUserData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetAdminUserByEmailResult>> GetAdminUserByEmail(GetAdminUserByEmailArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Admin/User")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAdminUserByEmailResult>();

            return AppResult<GetAdminUserByEmailResult>.CreateSucceeded(result, "Successfully called get admin user by email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAdminUserByEmailResult>.CreateFailed(ex, "An error occured when caling get admin user by email api");
        }
    }
}
