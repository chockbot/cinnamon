using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit;

public class VerifyCallbackHandler : IVerifyCallbackHandler
{
    private readonly ApplicationConfig applicationConfig;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IFinishTransactionHandler finishTransactionHandler;

    public VerifyCallbackHandler(ApplicationConfig applicationConfig, IPurchaseOrderData purchaseOrderData,
        IFinishTransactionHandler finishTransactionHandler)
    {
        this.applicationConfig = applicationConfig;
        this.purchaseOrderData = purchaseOrderData;
        this.finishTransactionHandler = finishTransactionHandler;
    }

    public AppResult<VerifyCallbackResult> Execute(VerifyCallbackArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyCallbackResult>.CreateFailed(ex, "An error occured in VerifyCallbackHandler"); 
        }
    }

    public async Task<AppResult<VerifyCallbackResult>> ExecuteAsync(VerifyCallbackArgs args)
    {
        try
        {
            // check callback token
            if(!applicationConfig.Payment.Accounts.First().Settings.Any(s => s.Name == "CallbackToken"))
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }

            var transactionId = int.Parse(args.TransactionId);
            var getPurchaseOrder = await purchaseOrderData.GetPurchaseOrderById(transactionId);
            if(!getPurchaseOrder.Succeeded || getPurchaseOrder.Result == null || !getPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Invalid transaction id"), "Invalid transaction id");
            }

            // status already changed can't be altered
            if(getPurchaseOrder.Result.Result.Status != 0)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }

            int status = args.Status switch 
            {
                "REQUIRES_ACTION" => 0,
                "PENDING" => 0,
                "AWAITING_CAPTURE" => 0,
                "SUCCEEDED" => 1,
                _ => 2
            };

            // no need to do something
            if(status == 0)
            {
                return AppResult<VerifyCallbackResult>.CreateSucceeded(new VerifyCallbackResult {}, "Success");
            }

            // update purchase order status
            var updatedPurchaseOrder = await purchaseOrderData.UpdatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.UpdatePurchaseOrderArgs {
                PurchaseOrderId = transactionId,
                Status = status
            });
            if(!updatedPurchaseOrder.Succeeded || updatedPurchaseOrder.Result == null || !updatedPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("An error occured"), "An error occured");
            }

            var finishResult = await finishTransactionHandler.ExecuteAsync(new TransactionService.Interactors.FinishTransactionArgs {
                TransactionId = transactionId,
            });
            if(!finishResult.Succeeded || finishResult.Result == null)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException(finishResult.Message), finishResult.Message);
            }

            return AppResult<VerifyCallbackResult>.CreateSucceeded(new VerifyCallbackResult {}, "An error occured in VerifyCallbackHandler");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyCallbackResult>.CreateFailed(ex, "An error occured in VerifyCallbackHandler");
        }
    }
}