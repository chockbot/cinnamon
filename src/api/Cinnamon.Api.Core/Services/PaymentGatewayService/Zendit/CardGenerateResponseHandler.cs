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

public class CardGenerateResponseHandler : IGenerateResponseHandler, ICardDriver
{
    private readonly ApplicationConfig applicationConfig;
    private readonly IFlurlClient flurlClient;

    public CardGenerateResponseHandler(ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac)
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

            if(args.CardDetails == null)
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // validate expiry month
            if(args.CardDetails.ExpiryMonth > 12 || args.CardDetails.ExpiryMonth <= 0)
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }
            string expiryMonth = args.CardDetails.ExpiryMonth.ToString().Length == 1 ? $"0{args.CardDetails.ExpiryMonth}" : args.CardDetails.ExpiryMonth.ToString();

            // validate expiry year
            var yearString = $"{DateTime.Now.Year.ToString().Substring(0,2)}{args.CardDetails.ExpiryYear}";
            args.CardDetails.ExpiryYear = int.Parse(yearString);
            if(args.CardDetails.ExpiryYear < DateTime.Now.Year || args.CardDetails.ExpiryYear.ToString().Length > 4)
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }
            string expiryYear = args.CardDetails.ExpiryYear.ToString();

            // validate card number
            args.CardDetails.CardNumber = args.CardDetails.CardNumber.Replace(" ","").Trim();
            if(args.CardDetails.CardNumber.Length != 16)
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // validate card cvv
            if(args.CardDetails.Cvv.Length != 3)
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // generate ids with 15 characters
            var referenceId = "000000000000000".Substring(args.TransactionId.ToString().Length) + args.TransactionId;

            var requestArgs = new RequestPaymentArgs {
                amount = args.Amount,
                country = "PH",
                currency = currency,
                reference_id = referenceId,
                payment_method = new RequestPaymentArgs.PaymentMethod {
                    type = "CARD",
                    reusability = "ONE_TIME_USE",
                    card = new RequestPaymentArgs.Card {
                        currency = currency,
                        channel_properties = new RequestPaymentArgs.Channel_Properties {
                            success_return_url = applicationConfig.FrontendUrl.AppendPathSegment("purchase/order").SetQueryParam("purchaseid", args.TransactionId),
                            cancel_return_url = applicationConfig.FrontendUrl,
                            failure_return_url = applicationConfig.FrontendUrl
                        },
                        card_information = new RequestPaymentArgs.CardInformation {
                            card_number = args.CardDetails.CardNumber,
                            cardholder_name = args.CardDetails.CardHolderName,
                            cvv = args.CardDetails.Cvv,
                            expiry_month = expiryMonth,
                            expiry_year = expiryYear,
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