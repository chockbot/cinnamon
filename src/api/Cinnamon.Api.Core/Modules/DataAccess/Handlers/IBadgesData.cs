using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;
using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;
public interface IBadgesData
{
    Task<AppResult<GetBadgeResult>> GetBadgeById(int id);
    Task<AppResult<GetAllBadgeResult>> GetAllBadges(GetAllBadgeArgs args);
    Task<AppResult<CreateBadgeResult>> CreateBadge(CreateBadgeArgs args);
    Task<AppResult<UpdateBadgeResult>> UpdateBadge(UpdateBadgeArgs args);
}
