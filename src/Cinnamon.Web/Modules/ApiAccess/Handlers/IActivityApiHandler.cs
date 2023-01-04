using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Activity.Response;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IActivityApiHandler 
{
    Task<AppResult<CreateActivityResult>> CreateActivity(CreateActivityArgs args, string token);
    Task<AppResult<GetExperienceTypesResult>> GetExperienceTypes();
    Task<AppResult<GetExperienceCategoriesResult>> GetExperienceCategories();
    Task<AppResult<GetSubCategoriesResult>> GetSubCategories();
} 