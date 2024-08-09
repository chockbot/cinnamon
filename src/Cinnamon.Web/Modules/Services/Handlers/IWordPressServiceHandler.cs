using Cinnamon.Web.Models.WordPress;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.Services.Handlers;

public interface IWordPressServicesHandler
{
        Task<AppResult<List<Post>>> GetPostsAsync();
        Task<AppResult<MediaPost>> GetMediaAsync(int mediaId);
}