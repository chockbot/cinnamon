using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCategory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Subcategory.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.SubCategory;

public class SubCategoryData: ISubCategoryData
{
    private readonly IFlurlClient flurlClient;
	public SubCategoryData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatedSubcategoryResult>> CreateSubCategory(CreateSubcategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("SubCategory/CreateSubCategory")
                .PostJsonAsync(args)
                .ReceiveJson<CreatedSubcategoryResult>();

            return AppResult<CreatedSubcategoryResult>.CreateSucceeded(result, "Successfully posting create subcategory api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedSubcategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedSubcategoryResult>.CreateFailed(ex, "An error occured when posting create subcategory api");
        }
    }
    public async Task<AppResult<GetAllSubcategoryResult>> GetAllSubCategory(GetAllSubcategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("SubCategory/GetAllSubCategory")
                            .SetQueryParams(
                                new
                                {
                                    countPerPage = args.CountPerPage,
                                    pageIndex = args.PageIndex
                                }).GetJsonAsync<GetAllSubcategoryResult>();

            return AppResult<GetAllSubcategoryResult>.CreateSucceeded(result, "Successfully getting get all subcategory api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllSubcategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllSubcategoryResult>.CreateFailed(ex, "An error occured when getting all subcategory api");
        }
    }

    public async Task<AppResult<GetSubcategorytResult>> GetSubCategoryById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"SubCategory/GetSubCategoryById/{id}")
                            .GetJsonAsync<GetSubcategorytResult>();

            return AppResult<GetSubcategorytResult>.CreateSucceeded(result, "Successfully getting subcategory by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSubcategorytResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSubcategorytResult>.CreateFailed(ex, "An error occured when getting subcategory by id api");
        }
    }

    public async Task<AppResult<UpdateSubcategoryResult>> UpdateSubCategory(UpdateSubcategoryArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("SubCategory/UpdateSubCategory")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateSubcategoryResult>();

            return AppResult<UpdateSubcategoryResult>.CreateSucceeded(result, "Successfully posting update subcategory api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateSubcategoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSubcategoryResult>.CreateFailed(ex, "An error occured when posting update subcategory api");
        }
    }
}
