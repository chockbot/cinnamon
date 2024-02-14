using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GetDisbursements : IGetDisbursements
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IDisbursementData disbursementData;

    public GetDisbursements(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IDisbursementData disbursementData)
    {
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.disbursementData = disbursementData;
    }

    public AppResult<GetDisbursementsResult> Execute(GetDisbursementsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetDisbursementsResult>> ExecuteAsync(GetDisbursementsArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<GetDisbursementsResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<GetDisbursementsResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var disbursementsRes = await disbursementData.GetDisbursementInformation(new Framework.ApiCommand.ApiData.Disbursement.Request.GetDisbursementInformationArgs {
                FilterBy = args.FilterBy,
                FilterValue = args.FilterValue
            });
            if(!disbursementsRes.Succeeded || disbursementsRes.Result is null || !disbursementsRes.Result.IsSuccess)
            {
                return AppResult<GetDisbursementsResult>.CreateFailed(new ApplicationException(disbursementsRes.Result?.ErrorInfo?.Message), disbursementsRes.Message);
            }

            return AppResult<GetDisbursementsResult>.CreateSucceeded(new GetDisbursementsResult {
                Disbursements = disbursementsRes.Result.Result.Select(d => new GetDisbursementsResult.Disbursement {
                    Amount = d.Amount,
                    Id = d.Id,
                    InclusivePayment = d.InclusivePayment,
                    Label = d.Label,
                    ProviderEmail = d.ProviderEmail,
                    ProviderFirstName = d.ProviderFirstName,
                    ProviderLastName = d.ProviderLastName,
                    Remarks = d.Remarks,
                    Status = d.Status
                })
            }, "Successfully get disbursement information.");
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementsResult>.CreateFailed(ex, "An error occured when getting disbursement information.");
        }
    }
}