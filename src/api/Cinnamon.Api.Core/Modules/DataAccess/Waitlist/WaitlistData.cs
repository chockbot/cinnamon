using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Cinnamon.Api.Core.Config;
using Flurl.Http.Configuration;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Response;

namespace Cinnamon.Api.Core.Modules.DataAccess.Waitlist;

public class WaitlistData : IWaitListData
{
    private readonly IFlurlClient flurlClient;

    public WaitlistData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetWaitlistResult>> GetWaitListByEmail(string email)
    {
        try
        {
            var result = await flurlClient
                            .Request($"WaitList/GetWaitlistByEmail/{email}")
                            .GetJsonAsync<GetWaitlistResult>();

            return AppResult<GetWaitlistResult>.CreateSucceeded(result, "Successfully posting create customer api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitlistResult>.CreateFailed(ex, "An error occured when posting create customer api");
        }
    }
}