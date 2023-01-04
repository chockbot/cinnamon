using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetSubCategoriesHandler : IGetSubCategoriesHandler
{
    private readonly ISubCategoryData subCategoryData;

    public GetSubCategoriesHandler(ISubCategoryData subCategoryData)
    {
        this.subCategoryData = subCategoryData;   
    }
    
    public AppResult<GetSubCategoriesResult> Execute(GetSubCategoriesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetSubCategoriesResult>.CreateFailed(ex, "An error occured in GetSubCategoriesHandler");
        }
    }

    public async Task<AppResult<GetSubCategoriesResult>> ExecuteAsync(GetSubCategoriesArgs args)
    {
        try
        {
            var result = await subCategoryData.GetAllSubCategory(new Framework.ApiCommand.ApiData.Subcategory.Request.GetAllSubcategoryArgs {});
            
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetSubCategoriesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetSubCategoriesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetSubCategoriesHandler");
            }

            return AppResult<GetSubCategoriesResult>.CreateSucceeded(new GetSubCategoriesResult {
                SubCategories = result.Result.Result.Select(s => {
                    return new GetSubCategoriesResult.SubCategory {
                        CategoryId = s.CategoryId,
                        Id = s.Id,
                        Name = s.SubCategory
                    };
                })
            }, "Successfully get sub categories");
        }
        catch (Exception ex)
        {
            return AppResult<GetSubCategoriesResult>.CreateFailed(ex, "An error occured in GetSubCategoriesHandler");
        }
    }
}