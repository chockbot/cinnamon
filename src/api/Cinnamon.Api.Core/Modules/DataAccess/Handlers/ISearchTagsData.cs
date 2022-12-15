using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Request;
using Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ISearchTagsData
{
    Task<AppResult<GetSearchTagsResult>> GetSearchTagsById(int id);
    Task<AppResult<GetAllSearchTagsResult>> GetAlSearchTags(GetAllSearchTagsArgs args);
    Task<AppResult<CreateSearchTagsResult>> CreateSearchTags(CreateSearchTagsArgs args);
    Task<AppResult<UpdateSearchTagsResult>> UpdateSearchTags(UpdateSearchTagsArgs args);
}
