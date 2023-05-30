using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Activity;

public class ActivityApiHandler : IActivityApiHandler
{
    private readonly IFlurlClient flurlClient;
    private readonly ILogger _logger;

    public ActivityApiHandler(IFlurlClientFactory flurlFac, Config.Config config, ILogger<ActivityApiHandler> logger)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
        _logger = logger;
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

    public async Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivitiesArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetAllActivities")
                .SetQueryParams(args)
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

    public async Task<AppResult<GetActivityResult>> GetOwnedActivityByHandler(string handler, string token, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/GetOwnedActivityByHandler/{handler}")
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

    public async Task<AppResult<GetActivityResult>> GetActivityByHandler(string handler, GetActivityArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request($"Activity/GetActivityByHandler/{handler}")
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

    public async Task<AppResult<UpdateActivityImageOrderResult>> UpdateActivityImageOrder(UpdateActivityImageOrderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateActivityImageOrder")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateActivityImageOrderResult>();

            return AppResult<UpdateActivityImageOrderResult>.CreateSucceeded(result, "Successfully update activity image order api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateActivityImageOrderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityImageOrderResult>.CreateFailed(ex, "An error occured when updating activity image order api");
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
                .Request("Activity/GetActivityImages")
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

    public async Task<AppResult<GetEnrolledActivitiesResult>> GetEnrolledActivities(GetEnrolledActivitiesArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/GetEnrolledActivities")
                .SetQueryParams(args)
                .GetJsonAsync<GetEnrolledActivitiesResult>();

            return AppResult<GetEnrolledActivitiesResult>.CreateSucceeded(result, "Successfully getting enrolled activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEnrolledActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledActivitiesResult>.CreateFailed(ex, "An error occured when getting enrolled activities api");
        }
    }

    public async Task<AppResult<GetAllRegionsResult>> GetAllRegions(GetAllRegionsArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/Regions")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllRegionsResult>();

            return AppResult<GetAllRegionsResult>.CreateSucceeded(result, "Successfully getting all regions api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllRegionsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllRegionsResult>.CreateFailed(ex, "An error occured when getting all regions api");
        }
    }

    public async Task<AppResult<GetAllCitiesResult>> GetAllCitiesByRegionCode(GetAllCitiesArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/Cities")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllCitiesResult>();

            return AppResult<GetAllCitiesResult>.CreateSucceeded(result, "Successfully getting all cities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllCitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllCitiesResult>.CreateFailed(ex, "An error occured when getting all cities api");
        }
    }

    public async Task<AppResult<GetAllBarangaysResult>> GetAllBarangaysByCityCode(GetAllBarangaysArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/Barangays")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllBarangaysResult>();

            return AppResult<GetAllBarangaysResult>.CreateSucceeded(result, "Successfully getting all barangays api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllBarangaysResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllBarangaysResult>.CreateFailed(ex, "An error occured when getting all barangays api");
        }
    }

    public async Task<AppResult<GetAllActivitiesResult>> GetPopularActivities(GetAllActivitiesArgs? args = null)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/Popular")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllActivitiesResult>();

            _logger.LogInformation("Cinnamon.Web > GetPopularActivities was called");

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

    public async Task<AppResult<GetRefundableExperienceResult>> GetRefundableExperience(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/GetRefundableExperience")
                .GetJsonAsync<GetRefundableExperienceResult>();

            return AppResult<GetRefundableExperienceResult>.CreateSucceeded(result, "Successfully getting all refundable experience api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetRefundableExperienceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetRefundableExperienceResult>.CreateFailed(ex, "An error occured when getting all refundable experience api");
        }
    }
    public async Task<AppResult<GetMakerActivitiesResult>> GetMakerActivities(GetMakerActivitiesArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/GetMakerActivities")
                .SetQueryParams(args)
                .GetJsonAsync<GetMakerActivitiesResult>();

            return AppResult<GetMakerActivitiesResult>.CreateSucceeded(result, "Successfully getting maker activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, "An error occured when getting maker activities api");
        }
    }

    public async Task<AppResult<UpdateScheduleResult>> UpdateActivitySchedule(UpdateScheduleArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/Schedule/Update")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateScheduleResult>();

            return AppResult<UpdateScheduleResult>.CreateSucceeded(result, "Successfully called update schedule api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateScheduleResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateScheduleResult>.CreateFailed(ex, "An error occured when calling update schedule api");
        }
    }

    public async Task<AppResult<DeleteActivityResult>> DeleteActivityById(DeleteActivityArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/Remove")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteActivityResult>();

            return AppResult<DeleteActivityResult>.CreateSucceeded(result, "Successfully called delete activity api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteActivityResult>.CreateFailed(ex, "An error occured when calling delete activity api");
        }
    }

    public async Task<AppResult<OwnerPricingInclusiveResult>> OwnerPricingInclusive(int id)
    {
        try
        {
            var result = await flurlClient
                .Request($"Activity/OwnerPricingInclusive/{id}")
                .GetJsonAsync<OwnerPricingInclusiveResult>();

            return AppResult<OwnerPricingInclusiveResult>.CreateSucceeded(result, "Successfully getting owner pricing inclusive identifier api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OwnerPricingInclusiveResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OwnerPricingInclusiveResult>.CreateFailed(ex, "An error occured when getting owner pricing inclusive identifier api");
        }   
    }
}