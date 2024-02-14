using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
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
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IStudentData studentData;
    private readonly IDisbursementData disbursementData;

    public VerifyPayoutCallbackHandler(IPayoutLogData payoutLogData, IPurchaseOrderData purchaseOrderData,
        ApplicationConfig applicationConfig, IJsonSerializationProvider jsonSerializationProvider,
        IStudentData studentData, IDisbursementData disbursementData)
    {
        this.payoutLogData = payoutLogData;
        this.purchaseOrderData = purchaseOrderData;
        this.applicationConfig = applicationConfig;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.studentData = studentData;
        this.disbursementData = disbursementData;
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
            var splittedReference = args.ReferenceId.Split("-");
            if(splittedReference.Count() <= 1)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid reference id"), "Invalid reference id");
            }

            var disbursementBulkId = Convert.ToInt32(splittedReference[1]);


            var getPayoutLogRes = await payoutLogData.GetPayoutLogById(transactionId);
            if(!getPayoutLogRes.Succeeded || getPayoutLogRes.Result == null || !getPayoutLogRes.Result.IsSuccess)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid reference id"), "Invalid reference id");
            }
            var payoutLog = getPayoutLogRes.Result.Result;

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

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(updatePayoutLog.Result.Result.Payload);

            // update only purchase order if callback status = 1
            if(status == 1 && deserializedPayload != null)
            {
                if(deserializedPayload.PurchaseOrderIds != null && deserializedPayload.PurchaseOrderIds.Count() > 0)
                {
                    var updatedPurchaseOrder = await purchaseOrderData.UpdatePurchaseOrdersStatus(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.UpdatePurchaseOrdersStatusArgs {
                        Ids = deserializedPayload.PurchaseOrderIds,
                        Status = 5
                    });
                    if(!updatedPurchaseOrder.Succeeded || updatedPurchaseOrder.Result == null || !updatedPurchaseOrder.Result.IsSuccess)
                    {
                        return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("An error occured. Please try again"), "An error occured. Please try again");
                    }
                }

                if(deserializedPayload.StudentIds != null && deserializedPayload.StudentIds.Count() > 0)
                {
                    var updateStudent = await studentData.UpdateStudentsDisbursementStatus(new Framework.ApiCommand.ApiData.Student.Request.UpdateStudentDisbursementArgs {
                        Ids = deserializedPayload.StudentIds,
                        IsDisbursement = true
                    });
                    if(!updateStudent.Succeeded || updateStudent.Result == null || !updateStudent.Result.IsSuccess)
                    {
                        return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("An error occured. Please try again"), "An error occured. Please try again");
                    }
                }
            }

            return AppResult<VerifyPayoutCallbackResult>.CreateSucceeded(new VerifyPayoutCallbackResult {}, "Success");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyPayoutCallbackResult>.CreateFailed(ex, "An error occured in VerifyPayoutCallbackHandler");
        }
    }

    class PayloadData 
    {
        public IEnumerable<int> PurchaseOrderIds {get; set;}
        public IEnumerable<int> StudentIds {get; set;}
    }
}   