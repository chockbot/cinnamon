using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExternalLoginToken.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ExternalLoginToken;

public class ExternalLoginTokenData: IExternalLoginTokenData
{
    private readonly IFlurlClient flurlClient;

	public ExternalLoginTokenData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetExternalLoginTokenResult>> GetLoginToken(GetLoginTokenArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"ExternalLoginToken/GetLoginToken")
                .PostJsonAsync(args)
                .ReceiveJson<GetExternalLoginTokenResult>();

            return AppResult<GetExternalLoginTokenResult>.CreateSucceeded(result, "Successfully get external login token api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExternalLoginTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExternalLoginTokenResult>.CreateFailed(ex, "An error occured when get external login token api");
        }
    }

    public async Task<AppResult<CreateExternalLoginTokenResult>> CreateToken(CreateExterLoginTokenArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"ExternalLoginToken/CreateToken")
                .PostJsonAsync(args)
                .ReceiveJson<CreateExternalLoginTokenResult>();

            return AppResult<CreateExternalLoginTokenResult>.CreateSucceeded(result, "Successfully create external login token api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateExternalLoginTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateExternalLoginTokenResult>.CreateFailed(ex, "An error occured when create external login token api");
        }
    }

    public async Task<AppResult<UpdateExternalLoginTokenResult>> UpdateToken(UpdateExternalLoginTokenArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"ExternalLoginToken/UpdateToken")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateExternalLoginTokenResult>();

            return AppResult<UpdateExternalLoginTokenResult>.CreateSucceeded(result, "Successfully update external login token api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateExternalLoginTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateExternalLoginTokenResult>.CreateFailed(ex, "An error occured when update external login token api");
        }
    }
}
