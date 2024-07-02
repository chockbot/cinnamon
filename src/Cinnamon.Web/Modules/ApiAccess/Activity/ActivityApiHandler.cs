using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ExperienceCreationType.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ExperienceCreationType.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Favorite.Response;
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
                    // activity images
                    if(args.Images is not null)
                    {
                        foreach(var image in args.Images)
                        {
                            if(image is not null)
                            {
                                mp.AddFile("Images", image.OpenReadStream(), image.FileName, image.ContentType);
                            }
                        }
                    }
                    // activity order position
                    if(args.Orders is not null)
                    {
                        foreach(var order in args.Orders)
                        {
                            mp.AddString("Orders", order.ToString());
                        }
                    }
                    // delete ids
                    if(args.DeletedIds is not null)
                    {
                        foreach(var id in args.DeletedIds)
                        {
                            mp.AddString("DeletedIds", id.ToString());
                        }
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

    public async Task<AppResult<GetCouponsResult>> GetCoupons(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/GetCoupons")
                .GetJsonAsync<GetCouponsResult>();

            return AppResult<GetCouponsResult>.CreateSucceeded(result, "Successfully getting coupons");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCouponsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCouponsResult>.CreateFailed(ex, "An error occured when getting coupons");
        }
    }

    public async Task<AppResult<CreateCouponResult>> CreateCoupon(CreateCouponArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/CreateCoupon")
                .PostJsonAsync(args)
                .ReceiveJson<CreateCouponResult>();

            return AppResult<CreateCouponResult>.CreateSucceeded(result, "Successfully called create coupon api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateCouponResult>.CreateFailed(ex, "An error occured when calling create coupon api");
        }
    }

    public async Task<AppResult<UpdateCouponStatusResult>> UpdateCouponStatus(UpdateCouponStatusArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateCouponStatus")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateCouponStatusResult>();

            return AppResult<UpdateCouponStatusResult>.CreateSucceeded(result, "Successfully called create coupon api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateCouponStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponStatusResult>.CreateFailed(ex, "An error occured when calling create coupon api");
        }
    }

    public async Task<AppResult<CreateFavoriteResult>> CreateFavorite(CreateFavoriteArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/Favorite/Create")
                .PostJsonAsync(args)
                .ReceiveJson<CreateFavoriteResult>();

            return AppResult<CreateFavoriteResult>.CreateSucceeded(result, "Successfully called create favorite api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateFavoriteResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateFavoriteResult>.CreateFailed(ex, "An error occured when calling create favorite api");
        }
    }

    public async Task<AppResult<RemoveFavoriteResult>> RemoveFavorite(RemoveFavoriteArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/Favorite/Remove")
                .PostJsonAsync(args)
                .ReceiveJson<RemoveFavoriteResult>();

            return AppResult<RemoveFavoriteResult>.CreateSucceeded(result, "Successfully called remove favorite api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<RemoveFavoriteResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RemoveFavoriteResult>.CreateFailed(ex, "An error occured when calling remove favorite api");
        }
    }

    public async Task<AppResult<GetFavoritesByCustomerResult>> GetFavoritesByCustomer(GetFavoritesByCustomerArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/Favorite/ByCustomer")
                .SetQueryParams(args)
                .GetJsonAsync<GetFavoritesByCustomerResult>();

            return AppResult<GetFavoritesByCustomerResult>.CreateSucceeded(result, "Successfully called get favorites api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetFavoritesByCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetFavoritesByCustomerResult>.CreateFailed(ex, "An error occured when calling get favorites api");
        }
    }

    public async Task<AppResult<ValidateCouponCodeResult>> ValidateCouponCode(ValidateCouponCodeArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/ValidateCouponCode")
                .PostJsonAsync(args)
                .ReceiveJson<ValidateCouponCodeResult>();

            return AppResult<ValidateCouponCodeResult>.CreateSucceeded(result, "Successfully called validate coupon api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<ValidateCouponCodeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ValidateCouponCodeResult>.CreateFailed(ex, "An error occured when calling validate coupon api");
        }
    }

    public async Task<AppResult<UpdateCouponResult>> UpdateCoupon(UpdateCouponArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateCoupon")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateCouponResult>();

            return AppResult<UpdateCouponResult>.CreateSucceeded(result, "Successfully called update coupon api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateCouponResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCouponResult>.CreateFailed(ex, "An error occured when calling update coupon api");
        }
    }

    public async Task<AppResult<RecommendedActivitiesResult>> RecommendedActivities(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/RecommendedActivities")
                .GetJsonAsync<RecommendedActivitiesResult>();

            return AppResult<RecommendedActivitiesResult>.CreateSucceeded(result, "Successfully called get recommended activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, "An error occured when calling get recommended activities api");
        }
    }

    public async Task<AppResult<PopularActivitiesResult>> PopularActivities(PopularActivitiesArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/PopularActivities")
                .SetQueryParams(args)
                .GetJsonAsync<PopularActivitiesResult>();

            return AppResult<PopularActivitiesResult>.CreateSucceeded(result, "Successfully called get popular activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<PopularActivitiesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<PopularActivitiesResult>.CreateFailed(ex, "An error occured when calling get popular activities api");
        }
    }

    public async Task<AppResult<GetExperienceCreationTypeResult>> GetExperienceCreationTypes(GetExperienceCreationTypeArgs args, string token)
    {
        try
        {
            var result = await flurlClient
               .WithOAuthBearerToken(token)
               .Request("Activity/ExperienceCreationTypes")
               .GetJsonAsync<GetExperienceCreationTypeResult>();

            return AppResult<GetExperienceCreationTypeResult>.CreateSucceeded(result, "Successfully getting experience creation types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceCreationTypeResult>.CreateFailed(ex, "An error occured when getting experience creation types api");
        }
    }

    public async Task<AppResult<GetActivityScheduleTimesResult>> GetActivityScheduleTimes(GetActivityScheduleTimesArgs args)
    {
        try
        {
            var result = await flurlClient
               .Request("Activity/GetActivityScheduleTimes")
               .SetQueryParams(args)
               .GetJsonAsync<GetActivityScheduleTimesResult>();

            return AppResult<GetActivityScheduleTimesResult>.CreateSucceeded(result, "Successfully getting activity schedule times api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityScheduleTimesResult>.CreateFailed(ex, "An error occured when getting activity schedule times api");
        }
    }

    public async Task<AppResult<CreateOngoingActivityScheduleResult>> CreateOngoingActivitySchedule(CreateOngoingActivityScheduleArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/CreateOngoingActivitySchedule")
                .PostJsonAsync(args)
                .ReceiveJson<CreateOngoingActivityScheduleResult>();

            return AppResult<CreateOngoingActivityScheduleResult>.CreateSucceeded(result, "Successfully called create ongoing activity schedule api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityScheduleResult>.CreateFailed(ex, "An error occured when calling create ongoing activity schedule api");
        }
    }
    public async Task<AppResult<CreateOteResult>> CreateOte(CreateOteArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/CreateOte")
                .PostJsonAsync(args)
                .ReceiveJson<CreateOteResult>();

            return AppResult<CreateOteResult>.CreateSucceeded(result, "Successfully called create one time event api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateOteResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateOteResult>.CreateFailed(ex, "An error occured when calling create one time event api");
        }
    }

    public async Task<AppResult<UpdateOteResult>> UpdateOte(UpdateOteArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateOte")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateOteResult>();

            return AppResult<UpdateOteResult>.CreateSucceeded(result, "Successfully called update one time event api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateOteResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOteResult>.CreateFailed(ex, "An error occured when calling update one time event api");
        }
    }

    public async Task<AppResult<OteActivityResult>> FindOteByHandler(OteActivityArgs args, string handler)
    {
        try
        {
            var result = await flurlClient
               .Request($"Activity/OteByHandler/{handler}")
               .SetQueryParams(args)
               .GetJsonAsync<OteActivityResult>();

            return AppResult<OteActivityResult>.CreateSucceeded(result, "Successfully getting ote activity by handler api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteActivityResult>.CreateFailed(ex, "An error occured when getting ote activity by handler api");
        }
    }

    public async Task<AppResult<OteTicketDetailsResult>> TicketDetails(string guid, string token)
    {
        try
        {
            var result = await flurlClient
               .Request($"Activity/TicketDetails/{guid}/{token}")
               .GetJsonAsync<OteTicketDetailsResult>();

            return AppResult<OteTicketDetailsResult>.CreateSucceeded(result, "Successfully getting ote activity ticket details");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteTicketDetailsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketDetailsResult>.CreateFailed(ex, "An error occured when getting ote activity ticket details");
        }
    }

    public async Task<AppResult<CustomerOteResult>> CustomerOte(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
               .Request($"Activity/CustomerOte")
               .GetJsonAsync<CustomerOteResult>();

            return AppResult<CustomerOteResult>.CreateSucceeded(result, "Successfully getting customer ote.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CustomerOteResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CustomerOteResult>.CreateFailed(ex, "An error occured when getting customer ote.");
        }
    }

    public async Task<AppResult<OteVerificationResult>> VerifyOTE(OteVerificationArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/VerifyOTE")
                .PostJsonAsync(args)
                .ReceiveJson<OteVerificationResult>();

            return AppResult<OteVerificationResult>.CreateSucceeded(result, "Successfully called OTE verification API");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<OteVerificationResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteVerificationResult>.CreateFailed(ex, "An error occured when calling OTE verification API");
        }
    }

    public async Task<AppResult<OtePerDayResult>> GetOtePerDay(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/GetOtePerDay")
                .GetJsonAsync<OtePerDayResult>();

            return AppResult<OtePerDayResult>.CreateSucceeded(result, "Successfully getting customer ote per day.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OtePerDayResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OtePerDayResult>.CreateFailed(ex, "An error occured when getting customer ote per day.");
        }
    }

    public async Task<AppResult<DeleteAddOnsResult>> DeleteAddOns(DeleteAddOnsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/DeleteAddOns")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteAddOnsResult>();

            return AppResult<DeleteAddOnsResult>.CreateSucceeded(result, "Successfully called delete add-on api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteAddOnsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(ex, "An error occurred when calling delete add-on api");
        }
    }

    public async Task<AppResult<DeleteAddOnResult>> DeleteAddOn(DeleteAddOnArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/DeleteAddOn")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteAddOnResult>();

            return AppResult<DeleteAddOnResult>.CreateSucceeded(result, "Successfully called delete add-on api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteAddOnResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAddOnResult>.CreateFailed(ex, "An error occurred when calling delete add-on api");
        }
    }

    public async Task<AppResult<GenerateEventSharedLinkResult>> GenerateEventSharedLink(GenerateEventSharedLinkArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/GenerateEventSharedLink")
                .PostJsonAsync(args)
                .ReceiveJson<GenerateEventSharedLinkResult>();

            return AppResult<GenerateEventSharedLinkResult>.CreateSucceeded(result, "Successfully called generate event shared link api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GenerateEventSharedLinkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GenerateEventSharedLinkResult>.CreateFailed(ex, "An error occurred when calling generate event shared link api");
        }
    }

    public async Task<AppResult<OteValidateSharedLinkResult>> ValidateSharedLink(OteValidateSharedLinkArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"Activity/ValidateSharedLink")
                .SetQueryParams(args)
                .GetJsonAsync<OteValidateSharedLinkResult>();

            return AppResult<OteValidateSharedLinkResult>.CreateSucceeded(result, "Successfully validate shared link.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteValidateSharedLinkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteValidateSharedLinkResult>.CreateFailed(ex, "An error occured when validate shared link.");
        }
    }

    public async Task<AppResult<VerifySharedEventLinkResult>> VerifySharedEventLink(VerifySharedEventLinkArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Activity/VerifySharedEventLink")
                .PostJsonAsync(args)
                .ReceiveJson<VerifySharedEventLinkResult>();

            return AppResult<VerifySharedEventLinkResult>.CreateSucceeded(result, "Successfully called OTE verification API");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<VerifySharedEventLinkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifySharedEventLinkResult>.CreateFailed(ex, "An error occured when calling OTE verification API");
        }
    }

    public async Task<AppResult<DeleteOnlineEventResult>> DeleteOnlineEvent(DeleteOnlineEventArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/DeleteOnlineEvent")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteOnlineEventResult>();

            return AppResult<DeleteOnlineEventResult>.CreateSucceeded(result, "Successfully called delete online event");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteOnlineEventResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteOnlineEventResult>.CreateFailed(ex, "An error occurred when calling delete online event api");
        }
    }

    public async Task<AppResult<UpdateSharedLinkStatusResult>> UpdateSharedLinkStatus(UpdateSharedLinkStatusArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateSharedLinkStatus")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateSharedLinkStatusResult>();

            return AppResult<UpdateSharedLinkStatusResult>.CreateSucceeded(result, "Successfully called generate event shared link api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateSharedLinkStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSharedLinkStatusResult>.CreateFailed(ex, "An error occurred when calling generate event shared link api");
        }
    }
    public async Task<AppResult<DeleteTicketResult>> DeleteTicket(DeleteTicketArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/DeleteTicket")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteTicketResult>();

            return AppResult<DeleteTicketResult>.CreateSucceeded(result, "Successfully called delete ticket");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteTicketResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteTicketResult>.CreateFailed(ex, "An error occurred when calling delete ticket api");
        }
    }

    public async Task<AppResult<ActivityFeedResult>> ActivityFeed(ActivityFeedArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"Activity/ActivityFeed")
                .SetQueryParams(args)
                .GetJsonAsync<ActivityFeedResult>();

            return AppResult<ActivityFeedResult>.CreateSucceeded(result, "Successfully get activity feed.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ActivityFeedResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ActivityFeedResult>.CreateFailed(ex, "An error occured when get activity feed.");
        }
    }

    public async Task<AppResult<OteAlreadyBookedDatesResult>> OteAlreadyBookedDates(int activityId, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/OteAlreadyBookedDates/{activityId}")
                .GetJsonAsync<OteAlreadyBookedDatesResult>();

            return AppResult<OteAlreadyBookedDatesResult>.CreateSucceeded(result, "Successfully ote already booked dates");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteAlreadyBookedDatesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteAlreadyBookedDatesResult>.CreateFailed(ex, "An error occured when ote already booked dates");
        }
    }

    public async Task<AppResult<OteScheduleDatesResult>> OteScheduleDates(OteScheduleDatesArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/OteScheduleDates")
                .SetQueryParams(args)
                .GetJsonAsync<OteScheduleDatesResult>();

            return AppResult<OteScheduleDatesResult>.CreateSucceeded(result, "Successfully get ote schedule dates.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteScheduleDatesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteScheduleDatesResult>.CreateFailed(ex, "An error occured when ote schedule dates.");
        }
    }

    public async Task<AppResult<OteBookedCountResult>> OteBookedCount(OteBookedCountArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/OteBookedCount")
                .SetQueryParams(args)
                .GetJsonAsync<OteBookedCountResult>();

            return AppResult<OteBookedCountResult>.CreateSucceeded(result, "Successfully get ote schedule dates.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<OteBookedCountResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteBookedCountResult>.CreateFailed(ex, "An error occured when ote schedule dates.");
        }
    }

    public async Task<AppResult<CreateOteWaitlistResult>> CreateOteWaitlist(CreateOteWaitlistArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/CreateOteWaitlist")
                .PostJsonAsync(args)
                .ReceiveJson<CreateOteWaitlistResult>();

            return AppResult<CreateOteWaitlistResult>.CreateSucceeded(result, "Successfully called create ote waitlist api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, "An error occured when calling create ote waitlist api");
        }
    }

    public async Task<AppResult<GetOteWaitlistByProviderResult>> GetOteWaitlistByProvider(GetOteWaitlistByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/GetWaitlistByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetOteWaitlistByProviderResult>();

            return AppResult<GetOteWaitlistByProviderResult>.CreateSucceeded(result, "Successfully get ote waitlist by provider.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, "An error occured when getting ote waitlist.");
        }
    }

    public async Task<AppResult<GetEmailTemplateResult>> GetEmailTemplate(GetEmailTemplateArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Activity/GetEmailTemplate")
                .SetQueryParams(args)
                .GetJsonAsync<GetEmailTemplateResult>();

            return AppResult<GetEmailTemplateResult>.CreateSucceeded(result, "Successfully get email template.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEmailTemplateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEmailTemplateResult>.CreateFailed(ex, "An error occured when getting email template.");
        }
    }
    
    public async Task<AppResult<DeleteOteWaitlistResult>> DeleteOteWaitlist(DeleteOteWaitlistArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/DeleteOteWaitlist")
                .PostJsonAsync(args)
                .ReceiveJson<DeleteOteWaitlistResult>();

            return AppResult<DeleteOteWaitlistResult>.CreateSucceeded(result, "Successfully called delete online event");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, "An error occurred when calling delete online event api");
        }
    }

    public async Task<AppResult<UpdateOteWaitlistResult>> UpdateOteWaitlist(UpdateOteWaitlistArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Activity/UpdateOteWaitlist")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateOteWaitlistResult>();

            return AppResult<UpdateOteWaitlistResult>.CreateSucceeded(result, "Successfully called update ote waitlist api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, "An error occured when calling uupdate ote waitlist api");
        }
    }
}