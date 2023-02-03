using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;
public interface IOngoingActivitiesHandler
{
    Task<AppResult<GetAllOngoingActivitiesResult>> GetAllOngoingActivities();

    Task<AppResult<GetOngoingActivityByIdResult>>GetOngoingActivityById(int id);

    Task<AppResult<UpdateOngoingActivityResult>> UpdateActivity(UpdateOngoingActivityArgs args);
}
