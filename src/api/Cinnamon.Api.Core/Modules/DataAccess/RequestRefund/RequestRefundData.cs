using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;
using Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.RequestRefund;

public class RequestRefundData : IRequestRefundData
{
    private readonly IFlurlClient flurlClient;

    public RequestRefundData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateRequestRefundResult>> CreateRequestRefund(CreateRequestRefundArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("RequestRefund/CreateRequestRefund")
                .PostJsonAsync(args)
                .ReceiveJson<CreateRequestRefundResult>();

            return AppResult<CreateRequestRefundResult>.CreateSucceeded(result, "Successfully posting create request refund api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateRequestRefundResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateRequestRefundResult>.CreateFailed(ex, "An error occured when posting create request refund api");
        }
    }

    public async Task<AppResult<GetAllRequestRefundResult>> GetAllRequestRefund(GetAllRequestRefundArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("RequestRefund/GetAllRequestRefund")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllRequestRefundResult>();

            return AppResult<GetAllRequestRefundResult>.CreateSucceeded(result, "Successfully getting get all request refund api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllRequestRefundResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllRequestRefundResult>.CreateFailed(ex, "An error occured when getting all request refund api");
        }
    }

    public async Task<AppResult<GetRequestRefundResult>> GetRequestRefundById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"RequestRefund/GetRequestRefundById/{id}")
                            .GetJsonAsync<GetRequestRefundResult>();

            return AppResult<GetRequestRefundResult>.CreateSucceeded(result, "Successfully getting request refund by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetRequestRefundResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetRequestRefundResult>.CreateFailed(ex, "An error occured when getting request refund by id api");
        }
    }

    public async Task<AppResult<UpdateRequestRefundResult>> UpdateRequestRefund(UpdateRequestRefundArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("RequestRefund/UpdateRequestRefund")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateRequestRefundResult>();

            return AppResult<UpdateRequestRefundResult>.CreateSucceeded(result, "Successfully posting update request refund api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateRequestRefundResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateRequestRefundResult>.CreateFailed(ex, "An error occured when posting update request refund api");
        }
    }
}