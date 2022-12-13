using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.SearchTag;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;
public interface ISearchTagsRepository
{
    Task<AppResult<SearchTagsDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<SearchTagsDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<SearchTagsDTO>>> GetAllAsync();
    Task<AppResult<SearchTagsDTO>> CreateSearchTagsAsync(int activityId, string searchTag1 , string searchTag2, string searchTag3, string searchTag4, string searchTag5);
    Task<AppResult<SearchTagsDTO>> UpdateSearchTagsAsync(int searchId,int? activityId, string? searchTag1, string? searchTag2, string? searchTag3, string? searchTag4, string? searchTag5);
}
