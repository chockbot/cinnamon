using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetExperienceCategoriesHandler : IGetExperienceCategoriesHandler
{
    private readonly IExperienceCategoryData experienceCategoryData;

    public GetExperienceCategoriesHandler(IExperienceCategoryData experienceCategoryData)
    {
        this.experienceCategoryData = experienceCategoryData;
    }

    public AppResult<GetExperienceCategoriesResult> Execute(GetExperienceCategoriesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceCategoriesResult>.CreateFailed(ex, "An error occured in GetExperienceCategoriesHandler");
        }
    }

    public async Task<AppResult<GetExperienceCategoriesResult>> ExecuteAsync(GetExperienceCategoriesArgs args)
    {
        try
        {
            var result = await experienceCategoryData.GetAllCategory(new Framework.ApiCommand.ApiData.ExperienceCategory.Request.GetAllCategoryArgs {});
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetExperienceCategoriesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetExperienceCategoriesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetExperienceCategoriesHandler");
            }

            return AppResult<GetExperienceCategoriesResult>.CreateSucceeded(new GetExperienceCategoriesResult {
                ExperienceCategories = result.Result.Result.Select(e => {
                    return new GetExperienceCategoriesResult.ExperienceCategory {
                        Category = e.Category,
                        IconPath = e.IconPath,
                        Id = e.Id
                    };
                })
            }, "Successfully get experience categories");
        }
        catch (Exception ex)
        {
            return AppResult<GetExperienceCategoriesResult>.CreateFailed(ex, "An error occured in GetExperienceCategoriesHandler");
        }
    }
}