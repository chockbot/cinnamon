using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IOngoingActivitiesData
{
    Task<AppResult<GetOngoingActivityResult>> GetOngoingActivityById(int id);
    Task<AppResult<GetAllOngoingActivityResult>> GetAllOngoingActivities(GetAllOngoingActivityArgs args);
    Task<AppResult<CreateOngoingActivityResult>> CreateOngoingActivity(CreateOngoingActivityArgs args);
    Task<AppResult<UpdateongoingActivityResult>> UpdateOngoingActivity(UpdateOngoingActivityArgs args);
}
