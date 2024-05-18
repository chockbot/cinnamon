using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IOteDateData 
{
    Task<AppResult<GetOteDateByIdResult>> GetOteDate(int activityId);

    Task<AppResult<GetOteDateResult>> GetOteDate(GetOteDateArgs args);
}