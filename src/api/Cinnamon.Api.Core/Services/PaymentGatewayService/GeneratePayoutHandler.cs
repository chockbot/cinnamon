using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Helpers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService;

public class GeneratePayoutHandler : IGeneratePayoutHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IPayoutLogData payoutLogData;
    private readonly IPayoutAccountData payoutAccountData;
    private readonly IActivityData activityData;
    private readonly IFlurlClient flurlClient;
    private readonly ApplicationConfig applicationConfig;
    private readonly GeneratePayoutHelper generatePayoutHelper;
    private readonly IJsonSerializationProvider jsonSerializationProvider;

    public GeneratePayoutHandler(IPurchaseOrderData purchaseOrderData, IPayoutLogData payoutLogData, 
        ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac, IPayoutAccountData payoutAccountData,
        IActivityData activityData, IJsonSerializationProvider jsonSerializationProvider)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.payoutLogData = payoutLogData;
        this.payoutAccountData = payoutAccountData;
        this.activityData = activityData;
        this.applicationConfig = applicationConfig;
        this.jsonSerializationProvider = jsonSerializationProvider;

        var paymentUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "DisbursementUrl").Value;
        flurlClient = flurlFac.Get(paymentUrl);

        this.generatePayoutHelper = new GeneratePayoutHelper();
    }

    public AppResult<GeneratePayoutResult> Execute(GeneratePayoutArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GeneratePayoutResult>.CreateFailed(ex, "An error occured in GeneratePayoutHandler");
        }
    }

    public async Task<AppResult<GeneratePayoutResult>> ExecuteAsync(GeneratePayoutArgs args)
    {
        try
        {
            var transactions = await purchaseOrderData.GetAllPurchaseOrderNeedToPayout();
            if(!transactions.Succeeded || transactions.Result == null || !transactions.Result.IsSuccess)
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException(transactions.Result?.ErrorInfo?.Message), transactions.Message);
            }

            if(!applicationConfig.Payment.Accounts.First().Settings.Any(a => a.Name == "Token"))
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException("Can't find authentication token"), "Can't find authentication token");
            }
            var authToken = applicationConfig.Payment.Accounts.First().Settings.First(a => a.Name == "Token").Value;
            
            // skip data have errors
            int totalTransactions = transactions.Result.Result.Count();
            for(int i = 0; i < totalTransactions; i++)
            {
                var transaction = transactions.Result.Result.ElementAt(i);
                if(transaction != null)
                {
                    // get activity
                    var activityRes = await activityData.GetActivityById(transaction.ActivityId);
                    if(!activityRes.Succeeded || activityRes.Result == null || !activityRes.Result.IsSuccess)
                    {
                        continue;
                    }
                    var activity = activityRes.Result.Result;

                    // get maker bank account
                    var accountRes = await payoutAccountData.GetPayoutAccountByCustomerId(activity.CreatedBy);
                    if(!accountRes.Succeeded || accountRes.Result == null || !accountRes.Result.IsSuccess)
                    {
                        continue;
                    }
                    var account = accountRes.Result.Result;

                    generatePayoutHelper.AddCustomerSummary(activity.CreatedBy, transaction.Total, transaction.Id, 
                        account.BankChannel, account.AccountHolder, account.AccountNumber);
                }
            }

            // generate payout log and send disbursement to xendit
            foreach(var summary in generatePayoutHelper.GetCustomerPayoutSummaries)
            {
                var objPayload = new {
                    PurchaseOrderIds = summary.PurchaseOrderIds
                };
                var serializePayload = jsonSerializationProvider.Serialize(objPayload);

                // create payout log
                var payoutLogres = await payoutLogData.CreatePayoutLog(new Framework.ApiCommand.ApiData.PayoutLog.Request.CreatePayoutLogArgs {
                    Amount = summary.TotalAmount,
                    CustomerId = summary.CustomerId,
                    PurchaseOrderId = 0,
                    Remarks = "Pending to zendit",
                    Status = 0,
                    Payload = serializePayload
                });
                if(!payoutLogres.Succeeded || payoutLogres.Result == null || !payoutLogres.Result.IsSuccess)
                {
                    continue;
                }
                var log = payoutLogres.Result.Result;

                // generate ids with 15 characters
                var referenceId = "000000000000000".Substring(log.Id.ToString().Length) + log.Id;
                var payoutRequest = new PayoutArgs {
                    amount = summary.TotalAmount,
                    channel_code = summary.BankChannel,
                    channel_properties = new PayoutArgs.ChannelProperties {
                        account_holder_name = summary.AccountHolder,
                        account_number = summary.AccountNumber
                    },
                    currency = "PHP",
                    reference_id = referenceId,
                };

                try
                {
                    var result = await flurlClient
                                .WithHeader("Authorization", $"Basic {authToken}")
                                .WithHeader("Idempotency-key", referenceId)
                                .Request()
                                .PostJsonAsync(payoutRequest)
                                .ReceiveJson();   
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();
                    
                    var updatedLog = await payoutLogData.UpdatePayoutLog(new Framework.ApiCommand.ApiData.PayoutLog.Request.UpdatePayoutLogArgs {
                        Id = log.Id,
                        Remarks = error.error_code,
                        Status = 2
                    });
                }
            }

            return AppResult<GeneratePayoutResult>.CreateSucceeded(new GeneratePayoutResult {}, "Successfully generate payout");
        }
        catch (Exception ex)
        {
            return AppResult<GeneratePayoutResult>.CreateFailed(ex, "An error occured in GeneratePayoutHandler");
        }
    }
}