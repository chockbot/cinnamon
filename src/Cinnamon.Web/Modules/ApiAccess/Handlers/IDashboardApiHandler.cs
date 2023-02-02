using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IDashboardApiHandler 
{
    Task<AppResult<GetActivitySchedulesResult>> GetActivitySchedules(string token);
} 
