using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GenerateDisbursementPayout : IGenerateDisbursementPayout
{
    private readonly IDisbursementData disbursementData;
    private readonly ApplicationConfig applicationConfig;
    private readonly IPayoutAccountData payoutAccountData;
    private readonly ICustomerPricingData customerPricingData;
    private readonly IFlurlClient flurlClient;
    
    private readonly Dictionary<int, PayoutAccountDTO> cachedPayoutAccounts = new();
    private readonly Dictionary<int, CustomerPricing> cachedCustomerPricing = new();
    private readonly Dictionary<int, CustomerPayoutSummary> cachedCustomerSummary = new();
    private readonly Dictionary<int, int> skippedProviderIds = new();

    public GenerateDisbursementPayout(IDisbursementData disbursementData, ApplicationConfig applicationConfig,
        IFlurlClientFactory flurlFac, IPayoutAccountData payoutAccountData, ICustomerPricingData customerPricingData)
    {
        this.disbursementData = disbursementData;
        this.applicationConfig = applicationConfig;
        this.payoutAccountData = payoutAccountData;
        this.customerPricingData = customerPricingData;

        var paymentUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "DisbursementUrl").Value;
        flurlClient = flurlFac.Get(paymentUrl);
    }

    public AppResult<GenerateDisbursementPayoutResult> Execute(GenerateDisbursementPayoutArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GenerateDisbursementPayoutResult>> ExecuteAsync(GenerateDisbursementPayoutArgs args)
    {
        try
        {
            if(!applicationConfig.Payment.Accounts.First().Settings.Any(a => a.Name == "Token"))
            {
                return AppResult<GenerateDisbursementPayoutResult>.CreateFailed(new ApplicationException("Can't find authentication token"), "Can't find authentication token");
            }
            var authToken = applicationConfig.Payment.Accounts.First().Settings.First(a => a.Name == "Token").Value;

            var disbursementsRes = await disbursementData.GetDisbursements(new Framework.ApiCommand.ApiData.Disbursement.Request.GetDisbursementsArgs {
                Count = int.MaxValue,
                Skip = 0,
                Status = "initiated"
            });
            if(!disbursementsRes.Succeeded || disbursementsRes.Result is null || !disbursementsRes.Result.IsSuccess)
            {
                return AppResult<GenerateDisbursementPayoutResult>.
                    CreateFailed(new ApplicationException(disbursementsRes.Result?.ErrorInfo?.Message), disbursementsRes.Message);
            }
            var disbursements = disbursementsRes.Result.Result;

            foreach(var disbursement in disbursements)
            {
                // skip activity providers don't have setup payout account.
                if(skippedProviderIds.ContainsKey(disbursement.CustomerId))
                {
                    continue;
                }
                if(!cachedPayoutAccounts.ContainsKey(disbursement.CustomerId))
                {
                    var payoutAccountRes = await payoutAccountData.GetPayoutAccountByCustomerId(disbursement.CustomerId);
                    if(!payoutAccountRes.Succeeded || payoutAccountRes.Result is null || !payoutAccountRes.Result.IsSuccess)
                    {
                        skippedProviderIds.Add(disbursement.CustomerId, disbursement.CustomerId);
                        continue;
                    }
                    cachedPayoutAccounts.Add(disbursement.CustomerId, payoutAccountRes.Result.Result);
                }
                var payoutAccount = cachedPayoutAccounts[disbursement.CustomerId];

                if(!cachedCustomerPricing.ContainsKey(disbursement.CustomerId))
                {
                    var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(disbursement.CustomerId);
                    if(!customerPricingRes.Succeeded || customerPricingRes.Result is null || !customerPricingRes.Result.IsSuccess)
                    {
                        cachedCustomerPricing.Add(disbursement.CustomerId, new CustomerPricing {
                            IsManualPayment = false,
                            MakerId = disbursement.CustomerId,
                            Rate = 0
                        });
                    }
                    else 
                    {
                        var customerPricingDetails = customerPricingRes.Result.Result;
                        cachedCustomerPricing.Add(disbursement.CustomerId, new CustomerPricing {
                            IsManualPayment = customerPricingDetails.IsManualPayment,
                            MakerId = disbursement.CustomerId,
                            Rate = customerPricingDetails.Rate
                        });
                    }
                }

                var customerPricing = cachedCustomerPricing[disbursement.CustomerId];
                if(!customerPricing.IsManualPayment)
                {
                    if(!cachedCustomerSummary.ContainsKey(disbursement.CustomerId))
                    {
                        cachedCustomerSummary.Add(disbursement.CustomerId, new CustomerPayoutSummary {
                            AccountHolder = payoutAccount.AccountHolder,
                            AccountNumber = payoutAccount.AccountNumber,
                            BankChannel = payoutAccount.BankChannel,
                            CustomerId = disbursement.CustomerId,
                        });
                    }

                    var customerSummary = cachedCustomerSummary[disbursement.CustomerId];
                    customerSummary.TotalAmount += disbursement.Amount;
                    customerSummary.SummaryDetails.Add(new SummaryDetails {
                        Amount = disbursement.Amount,
                        DisbursementId = disbursement.Id
                    });
                }
            }

            foreach(var customerSummary in cachedCustomerSummary.Values)
            {
                var disbursementBulkRes = await disbursementData.CreateDisbursementBulk(new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkArgs {
                    DisbursementBulk = new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkArgs.DisbursementBulkArgs {
                        Amount = customerSummary.TotalAmount,
                        CustomerId = customerSummary.CustomerId,
                        Remarks = string.Empty,
                        Status = "pending",
                        DisbursementDetailBulks = customerSummary.SummaryDetails.Select(d => {
                            return new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkArgs.DisbursementDetailBulkArgs {
                                Amount = d.Amount,
                                DisbursementId = d.DisbursementId
                            };
                        })
                    }
                });
                if(!disbursementBulkRes.Succeeded || disbursementBulkRes.Result is null || !disbursementBulkRes.Result.IsSuccess)
                {
                    continue;
                }
                var disbursementBulk = disbursementBulkRes.Result.Result;

                var referenceId = "DPO-" + "000000000000000".Substring(disbursementBulk.Id.ToString().Length) + disbursementBulk.Id;
                var payoutRequest = new PayoutArgs {
                    amount = customerSummary.TotalAmount,
                    channel_code = customerSummary.BankChannel,
                    channel_properties = new PayoutArgs.ChannelProperties {
                        account_holder_name = customerSummary.AccountHolder,
                        account_number = customerSummary.AccountNumber
                    },
                    currency = "PHP",
                    reference_id = referenceId
                };

                try
                {
                    var pgRequest = await flurlClient.WithHeader("Authorization", $"Basic {authToken}")
                                                     .WithHeader("Idempotency-key", referenceId)
                                                     .Request()
                                                     .PostJsonAsync(payoutRequest)
                                                     .ReceiveJson();
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();
                    
                    var updateBulkStatus = disbursementData.UpdateDisbursementBulkStatus(new Framework.ApiCommand.ApiData.Disbursement.Request.UpdateDisbursementBulkStatusArgs {
                        DisbursementBulkId = disbursementBulk.Id,
                        DisbursementBulkStatus = "error",
                        DisbursementStatus = "initiated",
                        Remarks = error.error_code
                    });

                    var createDisbursementBulkLogRes = disbursementData.CreateDisbursementBulkLog(new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkLogArgs {
                        DisbursementBulkLog = new Framework.ApiCommand.ApiData.Disbursement.Request.CreateDisbursementBulkLogArgs.DisbursementBulkLogArgs {
                            DisbursementBulkId = disbursementBulk.Id,
                            RefferenceId = referenceId,
                            Remarks = "Error when first initiate to payment gateway",
                            Status = "error"
                        }
                    });
                }
            }

            return AppResult<GenerateDisbursementPayoutResult>.CreateSucceeded(new(), "Successfully generate payout disbursement.");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateDisbursementPayoutResult>.CreateFailed(ex, "An error occured when generating disbursement payout.");
        }
    }

    private record CustomerPricing 
    {
        public int MakerId {get; set;}
        public decimal Rate {get; set;}
        public bool IsManualPayment {get; set;}
    }

    private record CustomerPayoutSummary
    {
        public int CustomerId {get; set;}
        public decimal TotalAmount {get; set;}
        public string BankChannel {get; set;}
        public string AccountHolder {get; set;}
        public string AccountNumber {get; set;}

        public List<SummaryDetails> SummaryDetails {get; set;} = new();
    }

    private record SummaryDetails 
    {
        public int DisbursementId {get; set;}
        public decimal Amount {get; set;}
    }
}