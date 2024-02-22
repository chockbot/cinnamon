using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GetDisbursementByProviderId : IGetDisbursementByProviderId
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IDisbursementData disbursementData;

    public GetDisbursementByProviderId(IGetProfileHandler getProfileHandler, IDisbursementData disbursementData)
    {
        this.getProfileHandler = getProfileHandler;
        this.disbursementData = disbursementData;
    }

    public AppResult<GetDisbursementByProviderResult> Execute(GetDisbursementByProviderArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetDisbursementByProviderResult>> ExecuteAsync(GetDisbursementByProviderArgs args)
    {
        try
        {
            var disbursementInfo = await disbursementData.GetDisbursementByProvider(new Framework.ApiCommand.ApiData.Disbursement.Request.GetDisbursementByProviderArgs
            {
                ProviderId   = args.ProviderId,
                FilterBy     = args.FilterBy,
                FilterValue  = args.FilterValue,
                CountPerPage = args.CountPerPage,
                PageIndex    = args.PageIndex    
            });
            if (!disbursementInfo.Succeeded || disbursementInfo.Result is null || !disbursementInfo.Result.IsSuccess)
            {
                return AppResult<GetDisbursementByProviderResult>.CreateFailed(new ApplicationException(disbursementInfo.Result?.ErrorInfo?.Message), disbursementInfo.Message);
            }

            return AppResult<GetDisbursementByProviderResult>.CreateSucceeded(new GetDisbursementByProviderResult
            {
                DisbursementInformation = disbursementInfo.Result.Result.Select(d => new GetDisbursementByProviderResult.DisbursementByProviderId
                {
                    Id                = d.Id,
                    Amount            = d.Amount,
                    InclusivePayment  = d.InclusivePayment,
                    Label             = d.Label,
                    Remarks           = d.Remarks,
                    Status            = d.Status,
                    Payload           = d.Payload,
                    PayoutDate        = d.PayoutDate,
                    ProviderId        = d.DisbursementInformation.ProviderId,
                    ProviderEmail     = d.DisbursementInformation.ProviderEmail,
                    CustomerName      = d.DisbursementInformation.CustomerName,
                    ProviderFirstName = d.DisbursementInformation.ProviderFirstName,
                    ProviderLastName  = d.DisbursementInformation.ProviderLastName
                }),
                ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                {
                    Code        = disbursementInfo?.Result?.ErrorInfo?.Code,
                    Description = disbursementInfo?.Result?.ErrorInfo?.Description,
                    Message     = disbursementInfo?.Result?.ErrorInfo?.Message
                },
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex    = disbursementInfo.Result.Pagination.PageIndex,
                    PerPage      = disbursementInfo.Result.Pagination.PerPage,
                    TotalPages   = disbursementInfo.Result.Pagination.TotalPages,
                    TotalRecords = disbursementInfo.Result.Pagination.TotalRecords
                }
            }, "Successfully get disbursement information.");
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementByProviderResult>.CreateFailed(ex, "An error occurred when getting disbursement information.");
        }
    }
}
