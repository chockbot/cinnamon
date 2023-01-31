using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;
public interface IOngoingActivitiesHandler
{
    Task<AppResult<GetAllOngoingActivitiesResult>> GetAllOngoingActivities(GetAllOngoingActivitiesResult? args = null);
}
