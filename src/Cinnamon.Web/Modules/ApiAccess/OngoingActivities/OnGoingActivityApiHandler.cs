using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;
using NuGet.Common;

namespace Cinnamon.Web.Modules.ApiAccess.OngoingActivities;

public class OnGoingActivityApiHandler: IOngoingActivitiesHandler
{
    private readonly IFlurlClient flurlClient;

	public OnGoingActivityApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
	{
        flurlClient = flurlFac.Get(config.ApiUrl);
    }
    public async Task<AppResult<AddActivityExpirationResult>> AddActivityExpiration(AddActivityExpirationArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OnGoingActivities/AddActivityExpiration")
                .PostJsonAsync(args)
                .ReceiveJson<AddActivityExpirationResult>();

            return AppResult<AddActivityExpirationResult>.CreateSucceeded(result, "Successfully posting update ongoing activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<AddActivityExpirationResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<AddActivityExpirationResult>.CreateFailed(ex, "An error occured when posting update ongoing activity api");
        }
    }

    public async Task<AppResult<GetAllOngoingActivitiesResult>> GetAllOngoingActivities()
    {
        try
        {
            var result = await flurlClient
                .Request("OnGoingActivities/GetAllOnGoingActivities")
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
    public async Task<AppResult<UpdateOngoingActivityResult>> UpdateActivity(UpdateOngoingActivityArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OnGoingActivities/UpdateOngoingActivity")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateOngoingActivityResult>();

            return AppResult<UpdateOngoingActivityResult>.CreateSucceeded(result, "Successfully posting update ongoing activity api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateOngoingActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOngoingActivityResult>.CreateFailed(ex, "An error occured when posting update ongoing activity api");
        }
    }
}
