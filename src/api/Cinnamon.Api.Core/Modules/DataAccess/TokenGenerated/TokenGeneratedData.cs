using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Request;
using Cinnamon.Framework.ApiCommand.ApiData.TokenGenerated.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.TokenGenerated;

public class TokenGeneratedData : ITokenGeneratedData
{
    private readonly IFlurlClient flurlClient;
	public TokenGeneratedData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateTokenResult>> CreateTokenGenerated(CreateTokenArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("TokenGenerated/")
                .PostJsonAsync(args)
                .ReceiveJson<CreateTokenResult>();

            return AppResult<CreateTokenResult>.CreateSucceeded(result, "Successfully posting create token generated api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateTokenResult>.CreateFailed(ex, "An error occured when posting create token generated api");
        }
    }

    public async Task<AppResult<GetTokenResult>> GetTokenGenerated(string guid, string token)
    {
        try
        {
            var result = await flurlClient
                            .Request($"TokenGenerated/{guid}/{token}")
                            .GetJsonAsync<GetTokenResult>();

            return AppResult<GetTokenResult>.CreateSucceeded(result, "Successfully getting get token generated.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetTokenResult>.CreateFailed(ex, "An error occured when getting token generated.");
        }
    }

    public async Task<AppResult<UpdateTokenResult>> UpdateToken(UpdateTokenArgs args, int id)
    {
        try
        {
            var result = await flurlClient
                .Request($"TokenGenerated/{id}")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateTokenResult>();

            return AppResult<UpdateTokenResult>.CreateSucceeded(result, "Successfully posting update token generated api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateTokenResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateTokenResult>.CreateFailed(ex, "An error occured when posting update token generated api");
        }
    }
}