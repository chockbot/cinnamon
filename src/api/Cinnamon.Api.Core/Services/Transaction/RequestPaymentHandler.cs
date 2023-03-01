using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Resolver;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class RequestPaymentHandler : IRequestPaymentHandler
{
    private readonly PGDriverResolver pGDriverResolver;
    private readonly ApplicationConfig applicationConfig;

    public RequestPaymentHandler(ApplicationConfig applicationConfig)
    {
        this.pGDriverResolver = new PGDriverResolver();
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<RequestPaymentResult> Execute(RequestPaymentArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<RequestPaymentResult>.CreateFailed(ex, "An error occured in RequestPaymentHandler");
        }
    }

    public async Task<AppResult<RequestPaymentResult>> ExecuteAsync(RequestPaymentArgs args)
    {
        try
        {
            var paymentMethod = applicationConfig.Payment.Accounts.First().PaymentMethods.FirstOrDefault(p => p.Name == args.PaymentMethod);
            if(paymentMethod == null)
            {
                return AppResult<RequestPaymentResult>.CreateFailed(new ApplicationException("Invalid Payment Method"), "Invalid Payment Method");
            }
            
            var paymentChannel = string.Empty;
            if(paymentMethod.Channels.Any(c => c.Code == args.PaymentChannel))
            {
                paymentChannel = args.PaymentChannel;
            }

            var pgHandler = (IGenerateResponseHandler)pGDriverResolver.ResolvePgDriver("IGenerateResponseHandler", paymentMethod.Driver);

            var result = await pgHandler.ExecuteAsync(new PaymentGatewayService.Interactors.GenerateResponseArgs {
                Amount = args.Amount,
                AmountCurrency = args.AmountCurrency,
                CustomerId = args.CustomerId,
                MetaDatas = args.MetaDatas,
                PaymentChannel = paymentChannel,
                TransactionId = args.TransactionId
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<RequestPaymentResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<RequestPaymentResult>.CreateSucceeded(new RequestPaymentResult {
                Action = result.Result.Action,
                Url = result.Result.Url
            }, "Successfully request payment");
        }
        catch (Exception ex)
        {
            return AppResult<RequestPaymentResult>.CreateFailed(ex, "An error occured in RequestPaymentHandler");
        }
    }
}