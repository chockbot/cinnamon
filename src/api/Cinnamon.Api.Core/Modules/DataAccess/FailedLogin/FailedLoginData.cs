using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;
using Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;
using Cinnamon.Api.Core.Config;

namespace Cinnamon.Api.Core.Modules.DataAccess.FailedLogin;

public class FailedLoginData : IFailedLoginData
{
    private readonly IFlurlClient flurlClient;

	public FailedLoginData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateFailedLoginResult>> CreateFailedLogin(CreateFailedLoginArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"FailedLogin/GetFailedLogins")
                .SetQueryParams(args)
                .GetJsonAsync<CreateFailedLoginResult>();

            return AppResult<CreateFailedLoginResult>.CreateSucceeded(result, "Successfully get failed logins api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateFailedLoginResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateFailedLoginResult>.CreateFailed(ex, "An error occured when get failed logins api");
        }
    }

    public async Task<AppResult<GetFailedLoginsResult>> GetFailedLogins(GetFailedLoginsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"FailedLogin/CreateFailedLogin")
                .PostJsonAsync(args)
                .ReceiveJson<GetFailedLoginsResult>();

            return AppResult<GetFailedLoginsResult>.CreateSucceeded(result, "Successfully create failed login api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetFailedLoginsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetFailedLoginsResult>.CreateFailed(ex, "An error occured when creating failed login api");
        }
    }
}