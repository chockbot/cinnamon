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
    }

    public AppResult<GenerateResponseResult> Execute(GenerateResponseArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured in GenerateResponseHandler-{ex.Message}");
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
                            success_return_url = applicationConfig.FrontendUrl.AppendPathSegment("purchase/order").SetQueryParam("purchaseid", args.TransactionId),
                            cancel_return_url = applicationConfig.FrontendUrl,
                            failure_return_url = applicationConfig.FrontendUrl
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
            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured in GenerateResponseHandler");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured in GenerateResponseHandler");
        }
    }
}