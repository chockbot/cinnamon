using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class ManualDisbursement : IManualDisbursement
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IDisbursementData disbursementData;

    public ManualDisbursement(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IDisbursementData disbursementData)
    {
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.disbursementData = disbursementData;
    }

    public AppResult<ManualDisbursementResult> Execute(ManualDisbursementArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ManualDisbursementResult>> ExecuteAsync(ManualDisbursementArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Remarks))
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(
                    new ApplicationException("Remarks required to manual disburse. Invalid request."), "Remarks required to manual disburse. Invalid request.");
            }

            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var disbursementRes = await disbursementData.GetDisbursementInformation(new Framework.ApiCommand.ApiData.Disbursement.Request.GetDisbursementInformationArgs {
                FilterBy = "disbursementId",
                FilterValue = args.DisbursementId.ToString()
            });
            if(!disbursementRes.Succeeded || disbursementRes.Result is null || !disbursementRes.Result.IsSuccess)
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(new ApplicationException(disbursementRes.Result?.ErrorInfo?.Message), disbursementRes.Message);
            }
            var disbursement = disbursementRes.Result.Result.FirstOrDefault();
            if(disbursement is null)
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(
                    new ApplicationException("Unable to find disbursement. Invalid request."), "Unable to find disbursement. Invalid request.");
            }

            if(disbursement.Status != "initiated")
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var updateDisbursementStatusRes = await disbursementData.UpdateDisbursementStatus(new Framework.ApiCommand.ApiData.Disbursement.Request.UpdateDisbursementStatusArgs {
                DisbursementId = disbursement.Id,
                Remarks = args.Remarks,
                Status = "disbursed"
            });
            if(!updateDisbursementStatusRes.Succeeded || updateDisbursementStatusRes.Result is null || !updateDisbursementStatusRes.Result.IsSuccess)
            {
                return AppResult<ManualDisbursementResult>.CreateFailed(
                    new ApplicationException(updateDisbursementStatusRes.Result?.ErrorInfo?.Message), updateDisbursementStatusRes.Message);
            }

            return AppResult<ManualDisbursementResult>.CreateSucceeded(new ManualDisbursementResult {}, "Successfully disbursed manually.");
        }
        catch (Exception ex)
        {
            return AppResult<ManualDisbursementResult>.CreateFailed(ex, "An error occured when manualing disbursement.");
        }
    }
}