using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
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
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFinishTransactionHandler oteFinishTransactionHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;

    public VerifyCallbackHandler(ApplicationConfig applicationConfig, IPurchaseOrderData purchaseOrderData,
        IFinishTransactionHandler finishTransactionHandler, IGetActivityHandler getActivityHandler,
        IOteFinishTransactionHandler oteFinishTransactionHandler, IJsonSerializationProvider jsonSerializationProvider)
    {
        this.applicationConfig = applicationConfig;
        this.purchaseOrderData = purchaseOrderData;
        this.finishTransactionHandler = finishTransactionHandler;
        this.getActivityHandler = getActivityHandler;
        this.oteFinishTransactionHandler = oteFinishTransactionHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
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
            var callbackToken = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "CallbackToken").Value;
            if(callbackToken != args.CallbackToken)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Invalid Request"), "Invalid Request");
            }

            var transactionId = int.Parse(args.TransactionId);
            var getPurchaseOrder = await purchaseOrderData.GetPurchaseOrderById(transactionId);
            if(!getPurchaseOrder.Succeeded || getPurchaseOrder.Result == null || !getPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Invalid transaction id"), "Invalid transaction id");
            }
            var purchaseOrder = getPurchaseOrder.Result.Result;

            // idenity what type of activity
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = purchaseOrder.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("Unable to identify activity id."), "Unable to identify activity id.");
            }
            var activity = activityRes.Result;

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

            var serializedPayload = jsonSerializationProvider.Serialize(args.Payload);

            // update purchase order status
            var updatedPurchaseOrder = await purchaseOrderData.UpdatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.UpdatePurchaseOrderArgs {
                PurchaseOrderId = transactionId,
                Status = status,
                PGPayload = serializedPayload
            });
            if(!updatedPurchaseOrder.Succeeded || updatedPurchaseOrder.Result == null || !updatedPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException("An error occured"), "An error occured");
            }

            if(status == 1)
            {
                if(activity.ExperienceCreationType == Framework.Enums.Enums.ExperienceCreationType.GeneralExperience)
                {
                    var finishResult = await finishTransactionHandler.ExecuteAsync(new TransactionService.Interactors.FinishTransactionArgs {
                        TransactionId = transactionId,
                    });
                    if(!finishResult.Succeeded || finishResult.Result == null)
                    {
                        return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException(finishResult.Message), finishResult.Message);
                    }
                }
                else if(activity.ExperienceCreationType == Framework.Enums.Enums.ExperienceCreationType.OneTimeEvents)
                {
                    var oteFinishResult  = await oteFinishTransactionHandler.ExecuteAsync(new TransactionService.Interactors.OteFinishTransactionArgs {
                        TransactionId = transactionId
                    });
                    if(!oteFinishResult.Succeeded || oteFinishResult.Result == null)
                    {
                        return AppResult<VerifyCallbackResult>.CreateFailed(new ApplicationException(oteFinishResult.Message), oteFinishResult.Message);
                    }
                }
            }

            return AppResult<VerifyCallbackResult>.CreateSucceeded(new VerifyCallbackResult {}, "Success! Payment Callback Validated");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyCallbackResult>.CreateFailed(ex, "An error occured in VerifyCallbackHandler");
        }
    }
}