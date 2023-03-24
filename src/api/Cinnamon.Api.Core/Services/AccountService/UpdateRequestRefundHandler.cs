using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class UpdateRequestRefundHandler : IUpdateRequestRefundHandler
{
    private readonly IRequestRefundData requestRefundData;

    public UpdateRequestRefundHandler(IRequestRefundData requestRefundData)
    {
        this.requestRefundData = requestRefundData;
    }
    
    public AppResult<UpdateRequestRefundResult> Execute(UpdateRequestRefundArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateRequestRefundResult>.CreateFailed(ex, "An error occured in UpdateRequestRefundHandler");
        }
    }

    public async Task<AppResult<UpdateRequestRefundResult>> ExecuteAsync(UpdateRequestRefundArgs args)
    {
        try
        {
            var result = await requestRefundData.UpdateRequestRefund(new Framework.ApiCommand.ApiData.RequestRefund.Request.UpdateRequestRefundArgs
            {
                RefundAmountGiven = args.RefundAmountGiven,
                RefundId          = args.RefundId,
                Status            = args.Status
            });

            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<UpdateRequestRefundResult>.CreateFailed(
                    new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            var refundResult = result.Result.Result;

            return AppResult<UpdateRequestRefundResult>.CreateSucceeded(new UpdateRequestRefundResult
            {
                Id              = refundResult.Id,
                Status          = refundResult.Status,
                ExperienceTitle = refundResult.ExperienceTitle,
                CustomerId      = refundResult.CustomerId,
                PurchaseOrderId = refundResult.PurchaseOrderId,
                Reason          = refundResult.Reason
            }, "Successfully updated request refund");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateRequestRefundResult>.CreateFailed(ex, "An error occured in UpdateRequestRefundHandler");
        }
    }
}