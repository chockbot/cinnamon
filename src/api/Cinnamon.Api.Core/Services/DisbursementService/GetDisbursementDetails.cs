using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GetDisbursementDetails : IGetDisbursementDetails
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IDisbursementData disbursementData;

    public GetDisbursementDetails(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IDisbursementData disbursementData)
    {
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.disbursementData = disbursementData;
    }
    
    public AppResult<GetDisbursementDetailsResult> Execute(GetDisbursementDetailsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetDisbursementDetailsResult>> ExecuteAsync(GetDisbursementDetailsArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var disbursementsRes = await disbursementData.GetDisbursementInformation(new Framework.ApiCommand.ApiData.Disbursement.Request.GetDisbursementInformationArgs {
                FilterBy = "disbursementId",
                FilterValue = args.DisbursementId.ToString()
            });
            if(!disbursementsRes.Succeeded || disbursementsRes.Result is null || !disbursementsRes.Result.IsSuccess)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(
                    new ApplicationException(disbursementsRes.Result?.ErrorInfo?.Message), disbursementsRes.Message);
            }

            var disbursement = disbursementsRes.Result.Result.FirstOrDefault();
            if(disbursement is null)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(
                    new ApplicationException("Unable to find disbursement. Invalid request."), "Unable to find disbursement. Invalid request.");
            }

            var disbursementDetailsRes = await disbursementData.GetDisbursementDetails(disbursement.Id);
            if(!disbursementDetailsRes.Succeeded || disbursementDetailsRes.Result is null || !disbursementDetailsRes.Result.IsSuccess)
            {
                return AppResult<GetDisbursementDetailsResult>.CreateFailed(
                    new ApplicationException(disbursementDetailsRes.Result?.ErrorInfo?.Message), disbursementDetailsRes.Message);
            }
            var disbursementItems = disbursementDetailsRes.Result.Result;

            return AppResult<GetDisbursementDetailsResult>.CreateSucceeded(new GetDisbursementDetailsResult {
                DisbursementDetailed = new GetDisbursementDetailsResult.Disbursement {
                    Amount = disbursement.Amount,
                    Id = disbursement.Id,
                    InclusivePayment = disbursement.InclusivePayment,
                    Label = disbursement.Label,
                    ProviderEmail = disbursement.ProviderEmail,
                    ProviderFirstName = disbursement.ProviderFirstName,
                    ProviderLastName = disbursement.ProviderLastName,
                    Remarks = disbursement.Remarks,
                    Status = disbursement.Status,
                    DisbursementDetails = disbursementItems.Select(d => new GetDisbursementDetailsResult.DisbursementDetail {
                        Amount = d.Amount,
                        Id = d.Id,
                        Label = d.Label
                    })
                }
            }, "Successfully get disbursement details.");
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementDetailsResult>.CreateFailed(ex, "An error occured when getting disbursement details.");
        }
    }
}