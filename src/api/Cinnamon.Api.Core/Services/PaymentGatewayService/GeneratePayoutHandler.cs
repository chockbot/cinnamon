using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
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

    public GeneratePayoutHandler(IPurchaseOrderData purchaseOrderData, IPayoutLogData payoutLogData, 
        ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac, IPayoutAccountData payoutAccountData,
        IActivityData activityData)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.payoutLogData = payoutLogData;
        this.payoutAccountData = payoutAccountData;
        this.activityData = activityData;
        this.applicationConfig = applicationConfig;

        var paymentUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "DisbursementUrl").Value;
        flurlClient = flurlFac.Get(paymentUrl);
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

                    // create payout log
                    var payoutLogres = await payoutLogData.CreatePayoutLog(new Framework.ApiCommand.ApiData.PayoutLog.Request.CreatePayoutLogArgs {
                        Amount = transaction.OverallTotal,
                        CustomerId = activity.CreatedBy,
                        PurchaseOrderId = transaction.Id,
                        Remarks = "Pending to zendit",
                        Status = 0
                    });
                    if(!payoutLogres.Succeeded || payoutLogres.Result == null || !payoutLogres.Result.IsSuccess)
                    {
                        continue;
                    }
                    var log = payoutLogres.Result.Result;

                    // generate ids with 15 characters
                    var referenceId = "000000000000000".Substring(log.Id.ToString().Length) + log.Id;

                    var payoutRequest = new PayoutArgs {
                        amount = transaction.OverallTotal,
                        channel_code = account.BankChannel,
                        channel_properties = new PayoutArgs.ChannelProperties {
                            account_holder_name = account.AccountHolder,
                            account_number = account.AccountNumber
                        },
                        currency = "PHP",
                        reference_id = referenceId,
                    };

                    var result = await flurlClient
                                    .WithHeader("Authorization", $"Basic {authToken}")
                                    .WithHeader("Idempotency-key", referenceId)
                                    .Request()
                                    .PostJsonAsync(payoutRequest)
                                    .ReceiveJson();
                }
            }

            return AppResult<GeneratePayoutResult>.CreateSucceeded(new GeneratePayoutResult {}, "Successfully generate payout");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GeneratePayoutResult>.CreateFailed(ex, $"An error occured in GenerateResponseHandler");
        }
        catch (Exception ex)
        {
            return AppResult<GeneratePayoutResult>.CreateFailed(ex, "An error occured in GeneratePayoutHandler");
        }
    }
}