using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.SearchTag;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.SearchTag;

public class SearchTagRepository: ISearchTagsRepository
{
    private readonly IDataStore dataStore;

	public SearchTagRepository(IDataStore dataStore)
	{
        this.dataStore = dataStore;
    }

    public async Task<AppResult<SearchTagsDTO>> CreateSearchTagsAsync(int activityId, string? searchTag1, string? searchTag2, string? searchTag3, string? searchTag4, string? searchTag5)
    {
        try
        {
            var searchTags = new Entities.SearchTags
            {
                ActivityId = activityId,
                SearchTag1 = searchTag1,    
                SearchTag2= searchTag2, 
                SearchTag3= searchTag3,
                SearchTag4= searchTag4, 
                SearchTag5= searchTag5  
            };
            var createdSearchTags = await dataStore.SearchTags.Add(searchTags);
            if (!createdSearchTags.Succeeded || createdSearchTags.Result == null)
            {
                return AppResult<SearchTagsDTO>.CreateFailed(createdSearchTags.Error.Exception, createdSearchTags.Message);
            }
            return AppResult<SearchTagsDTO>.CreateSucceeded(new SearchTagsDTO
            {
                ActivityId=activityId,
                SearchTag1= searchTag1,
                SearchTag2 = searchTag2,
                SearchTag3 = searchTag3,
                SearchTag4 = searchTag4,
                SearchTag5 = searchTag5
            }, "Successfully created experience category");
        }
        catch (Exception ex)
        {
            return AppResult<SearchTagsDTO>.CreateFailed(ex, "An error occured when creating experience category");
        }
    }

    public async Task<AppResult<IEnumerable<SearchTagsDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.SearchTags.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<SearchTagsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var category = result.Result.Select(c =>
            {
                return new SearchTagsDTO
                {
                    Id = c.Id,
                    ActivityId = c.ActivityId,
                    SearchTag1 = c.SearchTag1,
                    SearchTag2 = c.SearchTag2,
                    SearchTag3 = c.SearchTag3,
                    SearchTag4 = c.SearchTag4,
                    SearchTag5 = c.SearchTag5
                };
            });

            return AppResult<IEnumerable<SearchTagsDTO>>.CreateSucceeded(category, "Successfully get search tags");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<SearchTagsDTO>>.CreateFailed(ex, "An error occured in getting search tags");
        }
    }

    public async Task<AppResult<IEnumerable<SearchTagsDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.SearchTags.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<SearchTagsDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var searchTags = result.Result.Select(a =>
            {
                return new SearchTagsDTO
                {
                    Id = a.Id,
                    ActivityId = a.ActivityId,
                    SearchTag1 = a.SearchTag1,
                    SearchTag2 = a.SearchTag2,
                    SearchTag3 = a.SearchTag3,
                    SearchTag4 = a.SearchTag4,
                    SearchTag5 = a.SearchTag5
                };
            });
            return AppResult<IEnumerable<SearchTagsDTO>>.CreateSucceeded(searchTags, "Successfully get search tags");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<SearchTagsDTO>>.CreateFailed(ex, "An error occured in getting search tags");
        }
    }

    public async Task<AppResult<SearchTagsDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.SearchTags.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<SearchTagsDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var searchTagsDTO = new SearchTagsDTO
            {
                Id         = result.Result.Id,
                ActivityId = result.Result.ActivityId,
                SearchTag1 = result.Result.SearchTag1,
                SearchTag2 = result.Result.SearchTag2,
                SearchTag3 = result.Result.SearchTag3,
                SearchTag4 = result.Result.SearchTag4,
                SearchTag5 = result.Result.SearchTag5,
            };

            return AppResult<SearchTagsDTO>.CreateSucceeded(searchTagsDTO, "Successfully getting search tag by id");
        }
        catch (Exception ex)
        {
            return AppResult<SearchTagsDTO>.CreateFailed(ex, "An error occured when getting search tag by id");
        }
    }
    
    public async Task<AppResult<SearchTagsDTO>> UpdateSearchTagsAsync(int id, int? activityId, string? searchTag1, string? searchTag2, string? searchTag3, string? searchTag4, string? searchTag5)
    {
        try
        {
            var searchTagRes = await dataStore.SearchTags.GetByIdAsync(id);
            if (!searchTagRes.Succeeded || searchTagRes.Result == null)
            {
                return AppResult<SearchTagsDTO>.CreateFailed(new ApplicationException("Can't find search tag to update"), "Can't find search tag to update");
            }

            var searchTags = searchTagRes.Result;
            searchTags.ActivityId = activityId ?? searchTags.ActivityId;
            searchTags.SearchTag1 = searchTag1 ?? searchTags.SearchTag1;
            searchTags.SearchTag2 = searchTag1 ?? searchTags.SearchTag2;
            searchTags.SearchTag3 = searchTag1 ?? searchTags.SearchTag3;
            searchTags.SearchTag4 = searchTag1 ?? searchTags.SearchTag4;
            searchTags.SearchTag5 = searchTag1 ?? searchTags.SearchTag5;

            var updatedsearchTags = await dataStore.SearchTags.Update(searchTags);
            if (!updatedsearchTags.Succeeded)
            {
                return AppResult<SearchTagsDTO>.CreateFailed(updatedsearchTags.Error.Exception, updatedsearchTags.Message);
            }
            return AppResult<SearchTagsDTO>.CreateSucceeded(new SearchTagsDTO
            {
                ActivityId = searchTags.ActivityId,
                SearchTag1 = searchTags.SearchTag1, 
                SearchTag2 = searchTags.SearchTag2,
                SearchTag3 = searchTags.SearchTag3,
                SearchTag4 = searchTags.SearchTag4,
                SearchTag5 = searchTags.SearchTag5,
            }, "Successfully updated search tags");
        }
        catch (Exception ex)
        {
            return AppResult<SearchTagsDTO>.CreateFailed(ex, "An error occured when updating search tags");
        }
    }
}

