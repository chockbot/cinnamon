using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IActivityApiHandler 
{
    Task<AppResult<CreateActivityResult>> CreateActivity(CreateActivityArgs args, string token);
    Task<AppResult<UpdateActivityResult>> UpdateActivity(UpdateActivityArgs args, string token);
    Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivitiesArgs? args = null);
    Task<AppResult<GetAllActivitiesResult>> GetPopularActivities(GetAllActivitiesArgs? args = null);
    Task<AppResult<GetExperienceTypesResult>> GetExperienceTypes();
    Task<AppResult<GetExperienceCategoriesResult>> GetExperienceCategories();
    Task<AppResult<GetSubCategoriesResult>> GetSubCategories();
    Task<AppResult<GetActivityImagesResult>> GetActivityImages();
    Task<AppResult<GetAddressResult>> GetAddress();
    Task<AppResult<GetOwnedActivitiesResult>> GetOwnedActivities(GetOwnedActivitiesArgs args, string token);
    Task<AppResult<GetActivityResult>> GetOwnedActivity(int id, string token, GetActivityArgs? args = null);
    Task<AppResult<GetActivityResult>> GetActivity(int id, GetActivityArgs? args = null);
    Task<AppResult<UploadActivityImageResult>> UploadActivityImages(UploadActivityImageArgs args, string token);
    Task<AppResult<UpdateActivityImageOrderResult>> UpdateActivityImageOrder(UpdateActivityImageOrderArgs args, string token);
    Task<AppResult<GetActivitiesByCategoriesResult>> GetActivitiesByCategories(int id, GetActivityArgs? args = null);
    Task<AppResult<GetEnrolledActivitiesResult>> GetEnrolledActivities(GetEnrolledActivitiesArgs args, string token);
    Task<AppResult<GetActivityResult>> GetOwnedActivityByHandler(string handler, string token, GetActivityArgs? args = null);
    Task<AppResult<GetActivityResult>> GetActivityByHandler(string handler, GetActivityArgs? args = null);
    Task<AppResult<GetAllRegionsResult>> GetAllRegions(GetAllRegionsArgs? args = null);
    Task<AppResult<GetAllCitiesResult>> GetAllCitiesByRegionCode(GetAllCitiesArgs? args = null);
    Task<AppResult<GetAllBarangaysResult>> GetAllBarangaysByCityCode(GetAllBarangaysArgs? args = null);
    Task<AppResult<GetRefundableExperienceResult>> GetRefundableExperience(string token);
} 