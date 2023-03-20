using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.PayoutLog;

public class PayoutLogData : IPayoutLogData
{
    private readonly IFlurlClient flurlClient;

    public PayoutLogData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatePayoutLogResult>> CreatePayoutLog(CreatePayoutLogArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("PayoutLog/CreatePayoutLog")
                .PostJsonAsync(args)
                .ReceiveJson<CreatePayoutLogResult>();

            return AppResult<CreatePayoutLogResult>.CreateSucceeded(result, "Successfully posting create payout log api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatePayoutLogResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatePayoutLogResult>.CreateFailed(ex, "An error occured when posting create payout log api");
        }
    }

    public async Task<AppResult<GetAllPayoutLogsResult>> GetAllPayoutLogs(GetAllPayoutLogsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("PayoutLog/GetAllPayoutLogs")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllPayoutLogsResult>();

            return AppResult<GetAllPayoutLogsResult>.CreateSucceeded(result, "Successfully getting get all payout logs api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllPayoutLogsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllPayoutLogsResult>.CreateFailed(ex, "An error occured when getting all payout logs api");
        }
    }

    public async Task<AppResult<GetPayoutLogResult>> GetPayoutLogById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"PayoutLog/GetPayoutLogById/{id}")
                            .GetJsonAsync<GetPayoutLogResult>();

            return AppResult<GetPayoutLogResult>.CreateSucceeded(result, "Successfully getting payout log by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetPayoutLogResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutLogResult>.CreateFailed(ex, "An error occured when getting payout log by id api");
        }
    }

    public async Task<AppResult<UpdatePayoutLogResult>> UpdatePayoutLog(UpdatePayoutLogArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("PayoutLog/UpdatePayoutLog")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdatePayoutLogResult>();

            return AppResult<UpdatePayoutLogResult>.CreateSucceeded(result, "Successfully posting update payout log api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdatePayoutLogResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdatePayoutLogResult>.CreateFailed(ex, "An error occured when posting update payout log api");
        }
    }
}