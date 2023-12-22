using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Helpers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;
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
    private readonly IStudentData studentData;
    private readonly ICustomerPricingData customerPricingData;

    public GeneratePayoutHandler(IPurchaseOrderData purchaseOrderData, IPayoutLogData payoutLogData, 
        ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac, IPayoutAccountData payoutAccountData,
        IActivityData activityData, IJsonSerializationProvider jsonSerializationProvider, IStudentData studentData,
        ICustomerPricingData customerPricingData)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.payoutLogData = payoutLogData;
        this.payoutAccountData = payoutAccountData;
        this.activityData = activityData;
        this.applicationConfig = applicationConfig;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.studentData = studentData;
        this.customerPricingData = customerPricingData;

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
            if(!applicationConfig.Payment.Accounts.First().Settings.Any(a => a.Name == "Token"))
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException("Can't find authentication token"), "Can't find authentication token");
            }
            var authToken = applicationConfig.Payment.Accounts.First().Settings.First(a => a.Name == "Token").Value;

            var cachedPayoutAccounts = new Dictionary<int, PayoutAccountDTO>();
            var cachedCustomerPricing = new Dictionary<int, CustomerPricing>();

            // for exclusive transactions
            var transactions = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsInclusive = false
            });
            if(!transactions.Succeeded || transactions.Result == null || !transactions.Result.IsSuccess)
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException(transactions.Result?.ErrorInfo?.Message), transactions.Message);
            }

            // skip data have errors
            int totalTransactions = transactions.Result.Result.Count();
            for(int i = 0; i < totalTransactions; i++)
            {
                var transaction = transactions.Result.Result.ElementAt(i);
                if(transaction != null)
                {
                    // get maker payout account and cache in memory
                    if(!cachedPayoutAccounts.ContainsKey(transaction.MakerId))
                    {
                        var accountRes = await payoutAccountData.GetPayoutAccountByCustomerId(transaction.MakerId);
                        if(!accountRes.Succeeded || accountRes.Result == null || !accountRes.Result.IsSuccess)
                        {
                            continue;
                        }
                        cachedPayoutAccounts.Add(transaction.MakerId, accountRes.Result.Result);
                    }
                    var account = cachedPayoutAccounts[transaction.MakerId];

                    var amountToDisburse = transaction.PerUnitDisburseAmount;
                    generatePayoutHelper.AddCustomerSummary(transaction.MakerId, amountToDisburse, transaction.TransactionId, 
                        account.BankChannel, account.AccountHolder, account.AccountNumber, transaction.StudentId);
                }
            }

            // for expired experiences
            var expiredXPTransactions = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsExpired = true
            });
            if(!expiredXPTransactions.Succeeded || expiredXPTransactions.Result == null || !expiredXPTransactions.Result.IsSuccess)
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException(expiredXPTransactions.Result?.ErrorInfo?.Message), expiredXPTransactions.Message);
            }

            // skip data have errors
            int expiredXPTotalTransactions = expiredXPTransactions.Result.Result.Count();
            for(int i = 0; i < expiredXPTotalTransactions; i++)
            {
                var transaction = expiredXPTransactions.Result.Result.ElementAt(i);
                if(transaction != null)
                {
                    // get maker payout account and cache in memory
                    if(!cachedPayoutAccounts.ContainsKey(transaction.MakerId))
                    {
                        var accountRes = await payoutAccountData.GetPayoutAccountByCustomerId(transaction.MakerId);
                        if(!accountRes.Succeeded || accountRes.Result == null || !accountRes.Result.IsSuccess)
                        {
                            continue;
                        }
                        cachedPayoutAccounts.Add(transaction.MakerId, accountRes.Result.Result);
                    }
                    var account = cachedPayoutAccounts[transaction.MakerId];

                    var amountToDisburse = transaction.PerUnitDisburseAmount;
                    generatePayoutHelper.AddCustomerSummary(transaction.MakerId, amountToDisburse, transaction.TransactionId, 
                        account.BankChannel, account.AccountHolder, account.AccountNumber, transaction.StudentId);
                }
            }


            // for inslusive transaction
            var inclusiveTransactions = await studentData.GetStudentsToDisburse(new Framework.ApiCommand.ApiData.Student.Request.GetStudentsToDisburseArgs {
                IsInclusive = true
            });
            if(!inclusiveTransactions.Succeeded || inclusiveTransactions.Result == null || !inclusiveTransactions.Result.IsSuccess)
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException(inclusiveTransactions.Result?.ErrorInfo?.Message), inclusiveTransactions.Message);
            }
            // skip data have errors
            int totalInclusiveTransactions = inclusiveTransactions.Result.Result.Count();
            for(int i = 0; i < totalInclusiveTransactions; i++)
            {
                var transaction = inclusiveTransactions.Result.Result.ElementAt(i);
                if(transaction != null)
                {
                    // get maker payout account and cache in memory
                    if(!cachedPayoutAccounts.ContainsKey(transaction.MakerId))
                    {
                        var accountRes = await payoutAccountData.GetPayoutAccountByCustomerId(transaction.MakerId);
                        if(!accountRes.Succeeded || accountRes.Result == null || !accountRes.Result.IsSuccess)
                        {
                            continue;
                        }
                        cachedPayoutAccounts.Add(transaction.MakerId, accountRes.Result.Result);
                    }

                    // get customer pricing and cached in memory
                    if(!cachedCustomerPricing.ContainsKey(transaction.MakerId))
                    {
                        var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(transaction.MakerId);
                        if(!customerPricingRes.Succeeded || customerPricingRes.Result == null || !customerPricingRes.Result.IsSuccess)
                        {
                            continue;
                        }
                        var cp = customerPricingRes.Result.Result;
                        cachedCustomerPricing.Add(transaction.MakerId, 
                            new CustomerPricing { IsManualPayment = cp.IsManualPayment, MakerId = cp.Id, Rate = cp.Rate });
                    }
                    var customerPricing = cachedCustomerPricing[transaction.MakerId];

                    // skip manual disbursement
                    if(!customerPricing.IsManualPayment)
                    {
                        // decimal amountToDeduct = 0;
                        // var percentage = customerPricing.Rate / 100;
                        // amountToDeduct = percentage * transaction.UnitPrice;

                        // var totalAmount = transaction.UnitPrice - amountToDeduct;
                        var account = cachedPayoutAccounts[transaction.MakerId];
                        var amountToDisburse = transaction.PerUnitDisburseAmount;
                        
                        generatePayoutHelper.AddCustomerSummary(transaction.MakerId, amountToDisburse, transaction.TransactionId, 
                            account.BankChannel, account.AccountHolder, account.AccountNumber, transaction.StudentId);
                    }
                }
            }

            // for ote events transaction
            var oteTransactions = await purchaseOrderData.GetOteNeedToDisburse();
            if(!oteTransactions.Succeeded || oteTransactions.Result == null || !oteTransactions.Result.IsSuccess)
            {
                return AppResult<GeneratePayoutResult>.CreateFailed(new ApplicationException(oteTransactions.Result?.ErrorInfo?.Message), oteTransactions.Message);
            }
            // skip data have errors
            int totalOteTransaction = oteTransactions.Result.Result.Count();
            for(int i = 0; i < totalOteTransaction; i++)
            {
                var transaction = oteTransactions.Result.Result.ElementAt(i);
                if(transaction != null)
                {
                    // get maker payout account and cache in memory
                    if(!cachedPayoutAccounts.ContainsKey(transaction.MakerId))
                    {
                        var accountRes = await payoutAccountData.GetPayoutAccountByCustomerId(transaction.MakerId);
                        if(!accountRes.Succeeded || accountRes.Result == null || !accountRes.Result.IsSuccess)
                        {
                            continue;
                        }
                        cachedPayoutAccounts.Add(transaction.MakerId, accountRes.Result.Result);
                    }
                    var account = cachedPayoutAccounts[transaction.MakerId];

                    var amountToDisburse = transaction.TotalDisburseAmount;
                        
                    generatePayoutHelper.AddCustomerSummary(transaction.MakerId, amountToDisburse, transaction.TransactionId, 
                        account.BankChannel, account.AccountHolder, account.AccountNumber, transaction.StudentId);
                }
            }

            // generate payout log and send disbursement to xendit
            foreach(var summary in generatePayoutHelper.GetCustomerPayoutSummaries)
            {
                var objPayload = new {
                    PurchaseOrderIds = summary.PurchaseOrderIds.Distinct(),
                    StudentIds = summary.StudentIds.Where(s => s > 0).Distinct()
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

    public class CustomerPricing 
    {
        public int MakerId {get; set;}
        public decimal Rate {get; set;}
        public bool IsManualPayment {get; set;}
    }
}