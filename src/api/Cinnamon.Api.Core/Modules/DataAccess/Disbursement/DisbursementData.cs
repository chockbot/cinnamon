using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Disbursement;

public class DisbursementData : IDisbursementData
{
    private readonly IFlurlClient flurlClient;

    public DisbursementData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateDisbursementResult>> CreateDisbursements(CreateDisbursementArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDisbursementResult>();
            return AppResult<CreateDisbursementResult>.CreateSucceeded(result, "Successfully posting create disbursement api");
        }
        catch (FlurlHttpException ex)
        {
            var flutError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDisbursementResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDisbursementResult>.CreateFailed(ex, "An error occured when posting create disbursement api");
        }
    }
}