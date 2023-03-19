using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IPayoutLogData 
{
    Task<AppResult<GetPayoutLogResult>> GetPayoutLogById(int id);
    Task<AppResult<GetAllPayoutLogsResult>> GetAllPayoutLogs(GetAllPayoutLogsArgs args);
    Task<AppResult<CreatePayoutLogResult>> CreatePayoutLog(CreatePayoutLogArgs args);
    Task<AppResult<UpdatePayoutLogResult>> UpdatePayoutLog(UpdatePayoutLogArgs args);
}