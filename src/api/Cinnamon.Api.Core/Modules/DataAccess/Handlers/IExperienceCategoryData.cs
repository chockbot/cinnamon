using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IExperienceCategoryData
{
    Task<AppResult<GetCategoryResult>> GetCategotyById(int id);
    Task<AppResult<GetAllCategoryResult>> GetAllCategory(GetAllCategoryArgs args);  
    Task<AppResult<CreatedCategoryResult>> CreateCategory(CreateCategoryArgs args);
    Task<AppResult<UpdatedCategoryResult>> UpdateCategory(UpdateCategoryArgs args);
}
