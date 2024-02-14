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
    private readonly ApplicationConfig applicationConfig;
    private readonly IDisbursementData disbursementData;

    public VerifyPayoutCallbackHandler(ApplicationConfig applicationConfig, IDisbursementData disbursementData)
    {
        this.applicationConfig = applicationConfig;
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
            var disbursementBulkRes = await disbursementData.GetDisbursementBulk(disbursementBulkId);
            if(!disbursementBulkRes.Succeeded || disbursementBulkRes.Result is null || !disbursementBulkRes.Result.IsSuccess)
            {
                return AppResult<VerifyPayoutCallbackResult>.CreateFailed(new ApplicationException("Invalid reference id"), "Invalid reference id");
            }
            var disbursementBulk = disbursementBulkRes.Result.Result;

            string status = args.Status switch 
            {
                "PENDING" => "pending",
                "ACCEPTED" => "accepted",
                "SUCCEEDED" => "disbursed",
                "FAILED" => "failed",
                _ => args.Status
            };

            var createBulkLog = disbursementData.CreateDisbursementBulkLog(new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkLogArgs {
                DisbursementBulkLog = new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkLogArgs.DisbursementBulkLogArgs {
                    DisbursementBulkId = disbursementBulk.Id,
                    RefferenceId = args.ReferenceId,
                    Status = status,
                    Remarks = args.FailureCode
                }
            });

            if(status == "disbursed")
            {
                var updateDisburseBulkStatusRes = await disbursementData.UpdateDisbursementBulkStatus(new Framework.ApiCommand.ApiData.Disbursement.Request.UpdateDisbursementBulkStatusArgs {
                    DisbursementBulkId = disbursementBulk.Id,
                    DisbursementBulkStatus = "disbursed",
                    DisbursementStatus = "disbursed",
                    Remarks = "successfully disbursed"
                });
                if(!updateDisburseBulkStatusRes.Succeeded || updateDisburseBulkStatusRes.Result is null || !updateDisburseBulkStatusRes.Result.IsSuccess)
                {
                    return AppResult<VerifyPayoutCallbackResult>.CreateFailed(
                        new ApplicationException("An error occured. Please try again."), "An error occured. Please try again.");
                }
            }
            else if(status == "failed")
            {
                var updateDisburseBulkStatusRes = await disbursementData.UpdateDisbursementBulkStatus(new Framework.ApiCommand.ApiData.Disbursement.Request.UpdateDisbursementBulkStatusArgs {
                    DisbursementBulkId = disbursementBulk.Id,
                    DisbursementBulkStatus = "failed",
                    DisbursementStatus = "initiated",
                    Remarks = $"Failed to disburse with error code: {args.FailureCode}"
                });
                if(!updateDisburseBulkStatusRes.Succeeded || updateDisburseBulkStatusRes.Result is null || !updateDisburseBulkStatusRes.Result.IsSuccess)
                {
                    return AppResult<VerifyPayoutCallbackResult>.CreateFailed(
                        new ApplicationException("An error occured. Please try again."), "An error occured. Please try again.");
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