using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.PurchaseOrder;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class GetPayoutsByProviderHandler : IGetPayoutsByProviderHandler
{
    private readonly IPayoutLogData payoutLogData;

    public GetPayoutsByProviderHandler(IPayoutLogData payoutLogData)
    {
        this.payoutLogData = payoutLogData;
    }

    public AppResult<GetPayoutsByProviderResult> Execute(GetPayoutsByProviderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutsByProviderResult>.CreateFailed(ex, "An error occured in GetPayoutsByProviderHandler");
        }
    }

    public async Task<AppResult<GetPayoutsByProviderResult>> ExecuteAsync(GetPayoutsByProviderArgs args)
    {
        try
        {
            var result = await payoutLogData.GetPayoutsByProvider(new Framework.ApiCommand.ApiData.PayoutLog.Request.GetPayoutsByProviderArgs
            {
                Id       = args.Id,
                DateFrom = args.DateFrom.ToString("yyyyMMddHHmmss"),
                Status   = args.Status
            });
            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetPayoutsByProviderResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }
            return AppResult<GetPayoutsByProviderResult>.CreateSucceeded(new GetPayoutsByProviderResult
            {
                PayoutsLog = result.Result.Result.Select(e =>
                {
                    return new GetPayoutsByProviderResult.PayoutLog
                    {
                        Id              = e.Id,
                        PurchaseOrderId = e.PurchaseOrderId,
                        CustomerId      = e.CustomerId,
                        Amount          = e.Amount,
                        Status          = e.Status,
                        PayoutDate      = e.PayoutDate 
                    };
                })
            }, "Successfully get gross sales");

        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutsByProviderResult>.CreateFailed(ex, "An error occurred in GetPayoutsByProviderHandler");
        }
    }
}
