using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;
using NuGet.Common;

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

    public async Task<AppResult<GetExperienceCategoriesResult>> GetExperienceCategories(){
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

            return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting all activities api");
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

            return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting owned activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting owned activity api");
        }   
    }

    public async Task<AppResult<GetActivityResult>> GetActivity(int id, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request($"Activity/GetActivity/{id}")
                .SetQueryParams(args)
                .GetJsonAsync<GetActivityResult>();

            return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity api");
        }  
    }

    public async Task<AppResult<UploadActivityImageResult>> UploadActivityImages(UploadActivityImageArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/UploadActivityImage")
                .PostMultipartAsync(mp => {
                    if(args.Image1 != null)
                    {
                        mp.AddFile("Image1", args.Image1.OpenReadStream(), args.Image1.FileName, args.Image1.ContentType);
                    }
                    if(args.Image2 != null)
                    {
                        mp.AddFile("Image2", args.Image2.OpenReadStream(), args.Image2.FileName, args.Image2.ContentType);
                    }
                    if(args.Image3 != null)
                    {
                        mp.AddFile("Image3", args.Image3.OpenReadStream(), args.Image3.FileName, args.Image3.ContentType);
                    }
                    mp.AddString("ActivityId", args.ActivityId.ToString());
                })
                .ReceiveJson<UploadActivityImageResult>();

            return AppResult<UploadActivityImageResult>.CreateSucceeded(result, "Successfully upload activity images api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UploadActivityImageResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UploadActivityImageResult>.CreateFailed(ex, "An error occured when uploading activity images api");
        } 
    }

    public async Task<AppResult<GetAddressResult>> GetAddress()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetAllAddress")
                .GetJsonAsync<GetAddressResult>();

            return AppResult<GetAddressResult>.CreateSucceeded(result, "Successfully getting activity addresses api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAddressResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAddressResult>.CreateFailed(ex, "An error occured when getting activity addresses api");
        }
    }

    public async Task<AppResult<GetActivityImagesResult>> GetActivityImages()
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetAllActivityImages")
                .GetJsonAsync<GetActivityImagesResult>();

            return AppResult<GetActivityImagesResult>.CreateSucceeded(result, "Successfully getting activity images api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityImagesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityImagesResult>.CreateFailed(ex, "An error occured when getting activity images api");
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

            return AppResult<GetActivitiesByCategoriesResult>.CreateSucceeded(result, "Successfully getting activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured when getting activities api");
        }
    }
}