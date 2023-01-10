using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Response;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Transaction;

public class TransactionApiHandler : ITransactionApiHandler
{
    private readonly IFlurlClient flurlClient;

    public TransactionApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<SubmitPurchaseOrderResult>> SubmitPurchaseOrder(SubmitPurchaseOrderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Transaction/SubmitPurchaseOrder")
                .PostJsonAsync(args)
                .ReceiveJson<SubmitPurchaseOrderResult>();

            return AppResult<SubmitPurchaseOrderResult>.CreateSucceeded(result, "Successfully posting submit purchase order api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<SubmitPurchaseOrderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<SubmitPurchaseOrderResult>.CreateFailed(ex, "An error occured when posting submit purchase order api");
        }
    }

    public async Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrder(int id, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Transaction/GetPurchaseOrder/{id}")
                .GetJsonAsync<GetPurchaseOrderResult>();

            return AppResult<GetPurchaseOrderResult>.CreateSucceeded(result, "Successfully getting purchase order api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occured when getting purchase order api");
        }
    }
}