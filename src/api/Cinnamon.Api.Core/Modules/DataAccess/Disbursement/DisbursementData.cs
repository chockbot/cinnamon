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
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDisbursementResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDisbursementResult>.CreateFailed(ex, "An error occured when posting create disbursement api");
        }
    }

    public async Task<AppResult<CreateDisbursementBulkResult>> CreateDisbursementBulk(CreateDisbursementBulkArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement/CreateDisbursementBulk")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDisbursementBulkResult>();
            return AppResult<CreateDisbursementBulkResult>.CreateSucceeded(result, "Successfully posting create disbursement bulk api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDisbursementBulkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDisbursementBulkResult>.CreateFailed(ex, "An error occured when posting create disbursement bulk api.");
        }
    }

    public async Task<AppResult<UpdateDisbursementBulkStatusResult>> UpdateDisbursementBulkStatus(UpdateDisbursementBulkStatusArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement/UpdateDisbursementBulkStatus")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateDisbursementBulkStatusResult>();
            return AppResult<UpdateDisbursementBulkStatusResult>.CreateSucceeded(result, "Successfully posting update disbursement bulk status api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateDisbursementBulkStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDisbursementBulkStatusResult>.CreateFailed(ex, "An error occured when posting update disbursement bulk status api.");
        }
    }

    public async Task<AppResult<CreateDisbursementBulkLogResult>> CreateDisbursementBulkLog(CreateDisbursementBulkLogArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement/CreateDisbursementBulkLog")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDisbursementBulkLogResult>();
            return AppResult<CreateDisbursementBulkLogResult>.CreateSucceeded(result, "Successfully posting create disbursement bulk log api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDisbursementBulkLogResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDisbursementBulkLogResult>.CreateFailed(ex, "An error occured when posting create disbursement bulk log api.");
        }
    }

    public async Task<AppResult<GetDisbursementsResult>> GetDisbursements(GetDisbursementsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetDisbursementsResult>();
            return AppResult<GetDisbursementsResult>.CreateSucceeded(result, "Successfully get disbursements api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDisbursementsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementsResult>.CreateFailed(ex, "An error occured when get disbursements api");
        }
    }

    public async Task<AppResult<GetDisbursementBulkResult>> GetDisbursementBulk(int disbursementBulkId)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Disbursement/DisbursmentBulks/{disbursementBulkId}")
                            .GetJsonAsync<GetDisbursementBulkResult>();
            return AppResult<GetDisbursementBulkResult>.CreateSucceeded(result, "Successfully get disbursement bulk api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDisbursementBulkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementBulkResult>.CreateFailed(ex, "An error occured when get disbursement bulk api");
        }
    }

    public async Task<AppResult<GetDisbursementInformationResult>> GetDisbursementInformation(GetDisbursementInformationArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Disbursement/GetDisbursementInformation")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetDisbursementInformationResult>();
            return AppResult<GetDisbursementInformationResult>.CreateSucceeded(result, "Successfully get disbursement information api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDisbursementInformationResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementInformationResult>.CreateFailed(ex, "An error occured when get disbursement information api");
        }
    }

    public async Task<AppResult<GetDisbursementDetailsResult>> GetDisbursementDetails(int disbursementId)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Disbursement/GetDisbursementDetails/{disbursementId}")
                            .GetJsonAsync<GetDisbursementDetailsResult>();
            return AppResult<GetDisbursementDetailsResult>.CreateSucceeded(result, "Successfully get disbursement details api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDisbursementDetailsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementDetailsResult>.CreateFailed(ex, "An error occured when get disbursement details api");
        }
    }

    public async Task<AppResult<UpdateDisbursementStatusResult>> UpdateDisbursementStatus(UpdateDisbursementStatusArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Disbursement/UpdateDisbursementStatus")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateDisbursementStatusResult>();
            return AppResult<UpdateDisbursementStatusResult>.CreateSucceeded(result, "Successfully posting update disbursement status api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateDisbursementStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDisbursementStatusResult>.CreateFailed(ex, "An error occured when posting update disbursement status api.");
        }
    }
}