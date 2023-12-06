using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit;

public class EWalletGenerateResponseHandler : IGenerateResponseHandler, IEWalletDriver
{
    private readonly ApplicationConfig applicationConfig;
    private readonly IFlurlClient flurlClient;

    public EWalletGenerateResponseHandler(ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac)
    {
        this.applicationConfig = applicationConfig;
        var paymentUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "PaymentUrl").Value;
        flurlClient = flurlFac.Get(paymentUrl);
    }

    public AppResult<GenerateResponseResult> Execute(GenerateResponseArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured. Please try again later.");
        }
    }

    public async Task<AppResult<GenerateResponseResult>> ExecuteAsync(GenerateResponseArgs args)
    {
        try
        {
            var acceptedCurrencies = new string[] {"PHP", "USD"};
            var currency = args.AmountCurrency.ToUpper();
            if(!acceptedCurrencies.Any(s => s == currency))
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid currency."), "Invalid currency.");
            }

            var acceptedPaymentCode = applicationConfig.Payment.Accounts.First().
                PaymentMethods
                .Where(p => p.Name == "EWALLET")
                .First()
                .Channels
                .Select(s => s.Code);

            var paymentChannel = args.PaymentChannel.ToUpper();
            if(!acceptedPaymentCode.Any(s => s == paymentChannel))
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid payment channel code."), "Invalid payment channel code.");
            }

            // generate ids with 15 characters
            var referenceId = "000000000000000".Substring(args.TransactionId.ToString().Length) + args.TransactionId;

            var requestArgs = new RequestPaymentArgs {
                amount = args.Amount,
                country = "PH",
                currency = currency,
                reference_id = referenceId,
                payment_method = new RequestPaymentArgs.PaymentMethod {
                    type = "EWALLET",
                    reusability = "ONE_TIME_USE",
                    country = "PH",
                    ewallet = new RequestPaymentArgs.EWallet {
                        channel_code = paymentChannel,
                        channel_properties = new RequestPaymentArgs.Channel_Properties {
                            success_return_url = args.SuccessUrl,
                            cancel_return_url = applicationConfig.FrontendUrl,
                            failure_return_url = args.FailedUrl
                        }
                    }
                }
            };

            if(!applicationConfig.Payment.Accounts.First().Settings.Any(a => a.Name == "Token"))
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Can't find authentication token"), "Can't find authentication token");
            }
            var authToken = applicationConfig.Payment.Accounts.First().Settings.First(a => a.Name == "Token").Value;

            var result = await flurlClient
                .WithHeader("Authorization", $"Basic {authToken}")
                .Request()
                .PostJsonAsync(requestArgs)
                .ReceiveJson<RequestPaymentResult>();
            
            return AppResult<GenerateResponseResult>.CreateSucceeded(new GenerateResponseResult {
                Action = result.Status == "REQUIRES_ACTION" ? 1 : 0,
                Url = result.Actions.First().Url
            }, "Successfully request payment");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync<ErrorResponse>();
            if(error.error_code == "API_VALIDATION_ERROR ")
            {
                var errorMessage = error.message;
                if(error.errors != null && error.errors.Count() > 1)
                {
                    errorMessage = error.errors.First().message;
                }
                return AppResult<GenerateResponseResult>.CreateFailed(ex, errorMessage);
            }

            if(error.error_code == "ACCOUNT_ACCESS_BLOCKED")
            {
                return AppResult<GenerateResponseResult>.CreateFailed(ex, "Access to your underlying account or card has been blocked by the partner channel or the issuer");
            }

            if(error.error_code == "INVALID_ACCOUNT_DETAILS")
            {
                return AppResult<GenerateResponseResult>.CreateFailed(ex, "The provided details were rejected by the partner channel due to incorrect information.");
            }

            if(error.error_code == "MAX_ACCOUNT_LINKING")
            {
                return AppResult<GenerateResponseResult>.CreateFailed(ex, "The direct debit account being attempted to be linked has reached the maximum linking allowed by the partner channel.");
            }

            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured. Please try again later.");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured. Please try again later.");
        }
    }
}