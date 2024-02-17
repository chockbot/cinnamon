using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IDisbursementData 
{
    Task<AppResult<CreateDisbursementResult>> CreateDisbursements(CreateDisbursementArgs args);
}