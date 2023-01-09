using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Customer.Response;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IActivityApiHandler 
{
    Task<AppResult<CreateActivityResult>> CreateActivity(CreateActivityArgs args, string token);
    Task<AppResult<UpdateActivityResult>> UpdateActivity(UpdateActivityArgs args, string token);
    Task<AppResult<GetAllActivitiesResult>> GetAllActivities();
    Task<AppResult<GetExperienceTypesResult>> GetExperienceTypes();
    Task<AppResult<GetExperienceCategoriesResult>> GetExperienceCategories();
    Task<AppResult<GetSubCategoriesResult>> GetSubCategories();
    Task<AppResult<GetOwnedActivitiesResult>> GetOwnedActivities(GetOwnedActivitiesArgs args, string token);
    Task<AppResult<GetActivityResult>> GetOwnedActivity(int id, string token, GetActivityArgs? args = null);
    Task<AppResult<UploadActivityImageResult>> UploadActivityImages(UploadActivityImageArgs args, string token);
} 