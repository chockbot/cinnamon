using Cinnamon.Framework.Common;
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
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occurred when getting purchase order api");
        }
    }

    public async Task<AppResult<GetGrossSalesByProviderResult>> GetGrossSalesByProvider(GetGrossSalesByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Transaction/GetGrossSalesByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetGrossSalesByProviderResult>();

            return AppResult<GetGrossSalesByProviderResult>.CreateSucceeded(result, "Successfully getting gross sales api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, "An error occurred when getting gross sales api");
        }
    }

    public async Task<AppResult<GetPayoutsByProviderResult>> GetPayoutsByProvider(GetPayoutsByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Transaction/GetPayoutsByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetPayoutsByProviderResult>();

            return AppResult<GetPayoutsByProviderResult>.CreateSucceeded(result, "Successfully getting payouts by provider api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GetPayoutsByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutsByProviderResult>.CreateFailed(ex, "An error occurred when getting payouts by provider api");
        }
    }

    public async Task<AppResult<SubmitOtePurchaseOrderResult>> SubmitOtePurchaseOrder(SubmitOtePurchaseOrderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Transaction/SubmitOtePurchaseOrder")
                .PostJsonAsync(args)
                .ReceiveJson<SubmitOtePurchaseOrderResult>();

            return AppResult<SubmitOtePurchaseOrderResult>.CreateSucceeded(result, "Successfully posting submit purchase order api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<SubmitOtePurchaseOrderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<SubmitOtePurchaseOrderResult>.CreateFailed(ex, "An error occured when posting submit purchase order api");
        }
    }

    public async Task<AppResult<OteGetPurchaseOrderResult>> GetOtePurchaseOrder(int id, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Transaction/GetOtePurchaseOrder/{id}")
                .GetJsonAsync<OteGetPurchaseOrderResult>();

            return AppResult<OteGetPurchaseOrderResult>.CreateSucceeded(result, "Successfully getting purchase order api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<OteGetPurchaseOrderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<OteGetPurchaseOrderResult>.CreateFailed(ex, "An error occurred when getting purchase order api");
        }
    }

    public async Task<AppResult<TransactionRedirectionResult>> TransactionRedirection(TransactionRedirectionArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request($"Transaction/TransactionRedirection")
                .SetQueryParams(args)
                .GetJsonAsync<TransactionRedirectionResult>();

            return AppResult<TransactionRedirectionResult>.CreateSucceeded(result, "Successfully getting transaction redirection.");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<TransactionRedirectionResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<TransactionRedirectionResult>.CreateFailed(ex, "An error occurred when getting transaction redirection.");
        }
    }

    public async Task<AppResult<GetDirectStudentSalesResult>> GetDirectStudentSales(GetDirectStudentSalesArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"Transaction/GetDirectStudentSales")
                .SetQueryParams(args)
                .GetJsonAsync<GetDirectStudentSalesResult>();

            return AppResult<GetDirectStudentSalesResult>.CreateSucceeded(result, "Successfully getting direct student sales api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<GetDirectStudentSalesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentSalesResult>.CreateFailed(ex, "An error occurred when getting direct student sales api");
        }
    }

    public async Task<AppResult<PaymentRequestResult>> PaymentRequest(PaymentRequestArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Transaction/PaymentRequest")
                .PostJsonAsync(args)
                .ReceiveJson<PaymentRequestResult>();

            return AppResult<PaymentRequestResult>.CreateSucceeded(result, "Successfully posting submit payment request api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<PaymentRequestResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<PaymentRequestResult>.CreateFailed(ex, "An error occured when posting submit payment request api");
        }
    }
}