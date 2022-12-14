using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ExperienceCategory;

public class CategoryData: IExperienceCategoryData
{
    private readonly IFlurlClient flurlClient;
	public CategoryData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}
    public async Task<AppResult<CreatedCategoryResult>> CreateCategory(CreateCategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Category/CreateCategory")
                .PostJsonAsync(args)
                .ReceiveJson<CreatedCategoryResult>();

            return AppResult<CreatedCategoryResult>.CreateSucceeded(result, "Successfully posting create category api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedCategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedCategoryResult>.CreateFailed(ex, "An error occured when posting create category api");
        }
    }

    public async Task<AppResult<GetAllCategoryResult>> GetAllCategory(GetAllCategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Category/GetAllCategory")
                            .SetQueryParams(
                                new
                                {
                                    countPerPage = args.CountPerPage,
                                    pageIndex = args.PageIndex
                                }).GetJsonAsync<GetAllCategoryResult>();

            return AppResult<GetAllCategoryResult>.CreateSucceeded(result, "Successfully getting get all category api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllCategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllCategoryResult>.CreateFailed(ex, "An error occured when getting all category api");
        }
    }

    public async Task<AppResult<GetCategoryResult>> GetCategotyById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Category/GetCategoryById/{id}")
                            .GetJsonAsync<GetCategoryResult>();

            return AppResult<GetCategoryResult>.CreateSucceeded(result, "Successfully getting category by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCategoryResult>.CreateFailed(ex, "An error occured when getting category by id api");
        }
    }

    public async Task<AppResult<UpdatedCategoryResult>> UpdateCategory(UpdateCategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Category/UpdateCategory")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdatedCategoryResult>();

            return AppResult<UpdatedCategoryResult>.CreateSucceeded(result, "Successfully posting update category api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdatedCategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdatedCategoryResult>.CreateFailed(ex, "An error occured when posting update category api");
        }
    }
}
