using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Request;
using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.SearchTags;

public class SearchTagsData : ISearchTagsData
{
    private readonly IFlurlClient flurlClient;

    public SearchTagsData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateSearchTagsResult>> CreateSearchTags(CreateSearchTagsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("SearchTags/CreateSearchTags")
                .PostJsonAsync(args)
                .ReceiveJson<CreateSearchTagsResult>();

            return AppResult<CreateSearchTagsResult>.CreateSucceeded(result, "Successfully posting create search tags api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateSearchTagsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateSearchTagsResult>.CreateFailed(ex, "An error occured when posting create search tags api");
        }
    }

    public async Task<AppResult<GetAllSearchTagsResult>> GetAllSearchTags(GetAllSearchTagsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("SearchTags/GetAllSearchTags")
                            .SetQueryParams(
                                new
                                {
                                    countPerPage = args.CountPerPage,
                                    pageIndex = args.PageIndex
                                }).GetJsonAsync<GetAllSearchTagsResult>();

            return AppResult<GetAllSearchTagsResult>.CreateSucceeded(result, "Successfully getting get all search tags api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllSearchTagsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllSearchTagsResult>.CreateFailed(ex, "An error occured when getting all search tags api");
        }
    }

    public async Task<AppResult<GetSearchTagsResult>> GetSearchTagsById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"SearchTags/GetSearchTagsById/{id}")
                            .GetJsonAsync<GetSearchTagsResult>();

            return AppResult<GetSearchTagsResult>.CreateSucceeded(result, "Successfully getting search tags by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSearchTagsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSearchTagsResult>.CreateFailed(ex, "An error occured when getting search tags by id api");
        }
    }
    
    public async Task<AppResult<GetSearchTagsResult>> GetSearchTagsByActivityId(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"SearchTags/GetSearchTagsByActivityId/{id}")
                            .GetJsonAsync<GetSearchTagsResult>();

            return AppResult<GetSearchTagsResult>.CreateSucceeded(result, "Successfully getting search tags by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSearchTagsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSearchTagsResult>.CreateFailed(ex, "An error occured when getting search tags by id api");
        }
    }

    public async Task<AppResult<UpdateSearchTagsResult>> UpdateSearchTags(UpdateSearchTagsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("SearchTags/UpdateSearchTags")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateSearchTagsResult>();

            return AppResult<UpdateSearchTagsResult>.CreateSucceeded(result, "Successfully posting update search tags api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateSearchTagsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSearchTagsResult>.CreateFailed(ex, "An error occured when posting update search tags api");
        }
    }
}
