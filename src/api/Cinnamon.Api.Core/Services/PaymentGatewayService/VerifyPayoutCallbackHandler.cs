using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService;

public class VerifyPayoutCallbackHandler : IVerifyPayoutCallbackHandler
{
    private readonly IPayoutLogData payoutLogData;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ApplicationConfig applicationConfig;

    public VerifyPayoutCallbackHandler(IPayoutLogData payoutLogData, IPurchaseOrderData purchaseOrderData,
        ApplicationConfig applicationConfig)
    {
        this.payoutLogData = payoutLogData;
        this.purchaseOrderData = purchaseOrderData;
        this.applicationConfig = applicationConfig;
    }

    public AppResult<VerifyPayoutCallbackResult> Execute(VerifyPayoutCallbackArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyPayoutCallbackResult>.CreateFailed(ex, "An error occured in VerifyPayoutCallbackHandler");
        }
    }

    public async Task<AppResult<VerifyPayoutCallbackResult>> ExecuteAsync(VerifyPayoutCallbackArgs args)
    {
        try
        {
            // verify callback
            if(!applicationConfig.Payment.Accounts.First().Settings.Any(s => s.Name == "CallbackToken"))
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }
            var callbackToken = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "CallbackToken").Value;
            if(callbackToken != args.CallbackToken)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }

            // get payout log data
            var transactionId = int.Parse(args.ReferenceId);
            var getPayoutLogRes = await payoutLogData.GetPayoutLogById(transactionId);
            if(!getPayoutLogRes.Succeeded || getPayoutLogRes.Result == null || !getPayoutLogRes.Result.IsSuccess)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid reference id"), "Invalid reference id");
            }
            var payoutLog = getPayoutLogRes.Result.Result;

            // get purchase order data
            var getPurchaseOrderRes = await purchaseOrderData.GetPurchaseOrderById(getPayoutLogRes.Result.Result.PurchaseOrderId);
            if(!getPurchaseOrderRes.Succeeded || getPurchaseOrderRes.Result == null || !getPurchaseOrderRes.Result.IsSuccess)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }
            var transaction = getPurchaseOrderRes.Result.Result;

            // can only update if transaction status is succeed
            if(transaction.Status != 1)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }

            int status = args.Status switch 
            {
                "PENDING" => 0,
                "ACCEPTED" => 0,
                "SUCCEEDED" => 1,
                _ => 2
            };

            // no need to do something
            if(status == 0)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateSucceeded(new VerifyPayoutCallbackResult {}, "Success");
            }

            // update payout log data
            var updatePayoutLog = await payoutLogData.UpdatePayoutLog(new Framework.ApiCommand.ApiData.PayoutLog.Request.UpdatePayoutLogArgs {
                Id = payoutLog.Id,
                Remarks = args.FailureCode ?? string.Empty,
                Status = status
            });
            if(!updatePayoutLog.Succeeded || updatePayoutLog.Result == null || !updatePayoutLog.Result.IsSuccess)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("An error occured. Please try again"), "An error occured. Please try again");
            }

            // update only purchase order if callback status = 1
            if(status == 1)
            {
                var updatedPurchaseOrder = await purchaseOrderData.UpdatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.UpdatePurchaseOrderArgs {
                    Status = 5,
                    PurchaseOrderId = transaction.Id
                });
                if(!updatedPurchaseOrder.Succeeded || updatedPurchaseOrder.Result == null || !updatedPurchaseOrder.Result.IsSuccess)
                {
                    return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("An error occured. Please try again"), "An error occured. Please try again");
                }
            }

            return AppResult<VerifyPayoutCallbackResult>.CreateSucceeded(new VerifyPayoutCallbackResult {}, "Success");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyPayoutCallbackResult>.CreateFailed(ex, "An error occured in VerifyPayoutCallbackHandler");
        }
    }
}