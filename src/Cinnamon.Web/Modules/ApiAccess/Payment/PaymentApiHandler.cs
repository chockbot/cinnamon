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
}