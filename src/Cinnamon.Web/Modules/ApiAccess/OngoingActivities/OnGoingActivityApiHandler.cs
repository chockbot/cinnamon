using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.OngoingActivities;

public class OnGoingActivityApiHandler: IOngoingActivitiesHandler
{
    private readonly IFlurlClient flurlClient;

	public OnGoingActivityApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
	{
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<GetAllOngoingActivitiesResult>> GetAllOngoingActivities(GetAllOngoingActivitiesResult? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("OnGoingActivities/GetAllOnGoingActivities")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllOngoingActivitiesResult>();

            return AppResult<GetAllOngoingActivitiesResult>.CreateSucceeded(result, "Successfully getting all ongoing activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(ex, "An error occured when getting all ongoing activities api");
        }
    }
    public async Task<AppResult<GetOngoingActivityByIdResult>>GetOngoingActivityById(int id)
    {
        try
        {
            var result = await flurlClient
                .Request($"OnGoingActivities/GetOngoingActivity/{id}")
                .GetJsonAsync<GetOngoingActivityByIdResult>();

            return AppResult<GetOngoingActivityByIdResult>.CreateSucceeded(result, "Successfully getting student activity by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOngoingActivityByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOngoingActivityByIdResult>.CreateFailed(ex, "An error occured when getting student activity by id api");
        }
    }
}
