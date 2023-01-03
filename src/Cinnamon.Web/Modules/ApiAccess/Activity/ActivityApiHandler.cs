using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Activity;

public class ActivityApiHandler : IActivityApiHandler
{
    private readonly IFlurlClient flurlClient;

    public ActivityApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }
    
    public async Task<AppResult<CreateActivityResult>> CreateActivity(CreateActivityArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/CreateActivity")
                .PostJsonAsync(args)
                .ReceiveJson<CreateActivityResult>();

            return AppResult<CreateActivityResult>.CreateSucceeded(result, "Successfully posting create activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occured when posting create activity api");
        }
    }
}