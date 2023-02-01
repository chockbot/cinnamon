using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Dashboard;

public class DashboardApiHandler : IDashboardApiHandler
{
    private readonly IFlurlClient flurlClient;

    public DashboardApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }
    
    public async Task<AppResult<GetActivitySchedulesResult>> GetActivitySchedules(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetActivitySchedules")
                .GetJsonAsync<GetActivitySchedulesResult>();

            return AppResult<GetActivitySchedulesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivitySchedulesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitySchedulesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }
}