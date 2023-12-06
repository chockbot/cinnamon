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
    private readonly IFlurlClient paymentMethodClient;
    private readonly ILogger logger;

    public CardGenerateResponseHandler(ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac, ILogger<CardGenerateResponseHandler> logger)
    {
        this.applicationConfig = applicationConfig;
        var paymentUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "PaymentUrl").Value;
        flurlClient = flurlFac.Get(paymentUrl);

        var paymentMethodUrl = applicationConfig.Payment.Accounts.First().Settings.First(s => s.Name == "CreatePaymentMethodUrl").Value;
        paymentMethodClient = flurlFac.Get(paymentMethodUrl);
        this.logger = logger;
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

            if(!applicationConfig.Payment.Accounts.First().Settings.Any(a => a.Name == "Token"))
            {
                return AppResult<GenerateResponseResult>.CreateFailed(new ApplicationException("Can't find authentication token"), "Can't find authentication token");
            }
            var authToken = applicationConfig.Payment.Accounts.First().Settings.First(a => a.Name == "Token").Value;

            // create payment request
            var paymentRequest = new {
                type = "CARD",
                card = new {
                    currency = currency,
                    channel_properties = new {
                        success_return_url = args.SuccessUrl,
                        cancel_return_url = applicationConfig.FrontendUrl,
                        failure_return_url = args.FailedUrl
                    },
                    card_information = new {
                        card_number = args.CardDetails.CardNumber,
                        cardholder_name = args.CardDetails.CardHolderName,
                        cvv = args.CardDetails.Cvv,
                        expiry_month = expiryMonth,
                        expiry_year = expiryYear,
                    }
                },
                reusability = "ONE_TIME_USE"
            };

            var paymentRequestResult = await paymentMethodClient
                .WithHeader("Authorization", $"Basic {authToken}")
                .Request()
                .PostJsonAsync(paymentRequest)
                .ReceiveJson<RequestPaymentResult>();

            // add delay, xendit api is not accurate when the payment method is activated
            await Task.Delay(5000);

            /* request payment api
            *  this is the main transaction payment
            */
            // generate ids with 15 characters
            var referenceId = "000000000000000".Substring(args.TransactionId.ToString().Length) + args.TransactionId;

            var requestArgs = new {
                amount = args.Amount,
                country = "PH",
                currency = currency,
                reference_id = referenceId,
                payment_method_id = paymentRequestResult.Id
            };

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
            if(error.error_code == "API_VALIDATION_ERROR")
            {
                var errorMessage = error.message;
                if(error.errors != null && error.errors.Count() > 0)
                {
                    errorMessage = error.errors.First().message.ToUpper();
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

            logger.LogError("Error in card generate handler");
            logger.LogError(error.message);
            logger.LogError(error.error_code);

            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured. Please try again later.");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in card generate handler");
            logger.LogError(ex.Message);

            return AppResult<GenerateResponseResult>.CreateFailed(ex, $"An error occured. Please try again later.");
        }
    }
}