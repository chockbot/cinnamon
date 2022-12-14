using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ISubCategoryData
{
    Task<AppResult<GetSubcategorytResult>> GetSubCategoryById(int id);
    Task<AppResult<GetAllSubcategoryResult>> GetAllSubCategory(GetAllSubcategoryArgs args);
    Task<AppResult<CreatedSubcategoryResult>> CreateSubCategory(CreateSubcategoryArgs args);
    Task<AppResult<UpdateSubcategoryResult>> UpdateSubCategory(UpdateSubcategoryArgs args);
}
