using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Activity;

public class ActivityData: IActivityData
{
    private readonly IFlurlClient flurlClient;
	public ActivityData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatedActivityResult>> CreateActivity(CreateActivityArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/CreateActivity")
                .PostJsonAsync(args)
                .ReceiveJson<CreatedActivityResult>();

            return AppResult<CreatedActivityResult>.CreateSucceeded(result, "Successfully posting create activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedActivityResult>.CreateFailed(ex, "An error occured when posting create activity api");
        }
    }

    public async Task<AppResult<GetActivitiesByCategoriesResult>> GetActivitiesByCategories(int id, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Activity/GetActivitiesByCategories/{id}")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetActivitiesByCategoriesResult>();

            return AppResult<GetActivitiesByCategoriesResult>.CreateSucceeded(result, "Successfully getting activity by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured when getting activity by id api");
        }
    }

    public async Task<AppResult<GetActivityResult>> GetActivityById(int id, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Activity/GetActivityById/{id}")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetActivityResult>();

            return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity by id api");
        }
    }

    public async Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivities args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Activity/GetAllActivities")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllActivitiesResult>();

            return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting get all activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting all activities api");
        }
    }

    public async Task<AppResult<UpdatedActivityResult>> UpdateActivity(UpdateActivity args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Activity/UpdateActivity")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdatedActivityResult>();

            return AppResult<UpdatedActivityResult>.CreateSucceeded(result, "Successfully posting update activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdatedActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdatedActivityResult>.CreateFailed(ex, "An error occured when posting update activity api");
        }
    }
}
