using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IDisbursementData 
{
    Task<AppResult<CreateDisbursementResult>> CreateDisbursements(CreateDisbursementArgs args);

    Task<AppResult<GetDisbursementsResult>> GetDisbursements(GetDisbursementsArgs args);

    Task<AppResult<CreateDisbursementBulkLogResult>> CreateDisbursementBulkLog(CreateDisbursementBulkLogArgs args);

    Task<AppResult<UpdateDisbursementBulkStatusResult>> UpdateDisbursementBulkStatus(UpdateDisbursementBulkStatusArgs args);

    Task<AppResult<CreateDisbursementBulkResult>> CreateDisbursementBulk(CreateDisbursementBulkArgs args);

    Task<AppResult<GetDisbursementBulkResult>> GetDisbursementBulk(int disbursementBulkId);

    Task<AppResult<GetDisbursementInformationResult>> GetDisbursementInformation(GetDisbursementInformationArgs args);
}