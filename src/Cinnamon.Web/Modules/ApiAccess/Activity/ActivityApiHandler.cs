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
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateActivityResult>.CreateFailed(ex, "An error occured when posting create activity api");
        }
    }

    public async Task<AppResult<UpdateActivityResult>> UpdateActivity(UpdateActivityArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateActivity")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateActivityResult>();

            return AppResult<UpdateActivityResult>.CreateSucceeded(result, "Successfully posting update activity api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured when posting update activity api");
        }
    }

    public async Task<AppResult<GetExperienceTypesResult>> GetExperienceTypes()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetExperienceTypes")
                .GetJsonAsync<GetExperienceTypesResult>();

            return AppResult<GetExperienceTypesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExperienceTypesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceTypesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetExperienceCategoriesResult>> GetExperienceCategories()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetExperienceCategories")
                .GetJsonAsync<GetExperienceCategoriesResult>();

            return AppResult<GetExperienceCategoriesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExperienceCategoriesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceCategoriesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetSubCategoriesResult>> GetSubCategories()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetSubCategories")
                .GetJsonAsync<GetSubCategoriesResult>();

            return AppResult<GetSubCategoriesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSubCategoriesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSubCategoriesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetAllActivitiesResult>> GetAllActivities()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetAllActivities")
                .GetJsonAsync<GetAllActivitiesResult>();

            return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetOwnedActivitiesResult>> GetOwnedActivities(GetOwnedActivitiesArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/GetOwnedActivities")
                .SetQueryParams(args)
                .GetJsonAsync<GetOwnedActivitiesResult>();

            return AppResult<GetOwnedActivitiesResult>.CreateSucceeded(result, "Successfully getting owned activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOwnedActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivitiesResult>.CreateFailed(ex, "An error occured when getting owned activities api");
        }
    }

    public async Task<AppResult<GetActivityResult>> GetOwnedActivity(int id, string token, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/GetOwnedActivity/{id}")
                .SetQueryParams(args)
                .GetJsonAsync<GetActivityResult>();

            return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }
}