using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Payment;

public class PaymentApiHandler : IPaymentApiHandler
{
    private readonly IFlurlClient flurlClient;

	public PaymentApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
	{
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<VerifyCallbackResult>> VerifyCallback(VerifyCallbackArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Payment/VerifyCallback")
                .PostJsonAsync(args)
                .ReceiveJson<VerifyCallbackResult>();

            return AppResult<VerifyCallbackResult>.CreateSucceeded(result, "Successfully posting verify callback api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<VerifyCallbackResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifyCallbackResult>.CreateFailed(ex, "An error occured when posting verify callback api");
        }
    }

    public async Task<AppResult<GetPaymentChannelsResult>> GetPaymentChannels()
    {
        try
        {
            var result = await flurlClient
                .Request("Payment/GetPaymentChannels")
                .GetJsonAsync<GetPaymentChannelsResult>();

            return AppResult<GetPaymentChannelsResult>.CreateSucceeded(result, "Successfully get payment channels api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetPaymentChannelsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPaymentChannelsResult>.CreateFailed(ex, "An error occured when get payment channels api");
        }
    }

    public async Task<AppResult<VerifyPayoutCallbackResult>> VerifyPayoutCallback(VerifyPayoutCallbackArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Payment/VerifyPayoutCallback")
                .PostJsonAsync(args)
                .ReceiveJson<VerifyPayoutCallbackResult>();

            return AppResult<VerifyPayoutCallbackResult>.CreateSucceeded(result, "Successfully posting verify callback api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<VerifyPayoutCallbackResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<VerifyPayoutCallbackResult>.CreateFailed(ex, "An error occured when posting verify callback api");
        }
    }
}