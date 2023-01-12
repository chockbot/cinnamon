using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ResendEmail.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ResendEmail;

public class ResendEmailData : IResendEmailData
{
    private readonly IFlurlClient flurlClient;

    public ResendEmailData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreatedEmailResendResult>> CreateEmailResend(CreateEmailResendArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResendEmail/CreateEmailResend")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreatedEmailResendResult>();

            return AppResult<CreatedEmailResendResult>.CreateSucceeded(result, "Successfully posting create resend email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreatedEmailResendResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreatedEmailResendResult>.CreateFailed(ex, "An error occured when posting create resend email api");
        }
    }

    public async Task<AppResult<GetAllResendEmailResult>> GetAllResendEmails(GetAllResendEmailArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResendEmail/GetAllResendEmail")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllResendEmailResult>();

            return AppResult<GetAllResendEmailResult>.CreateSucceeded(result, "Successfully getting get all resend emails api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllResendEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllResendEmailResult>.CreateFailed(ex, "An error occured when getting all resend emails api");
        }
    }

    public async Task<AppResult<GetResendEmailByEmailDateRange>> GetResendEmailByDataRange(GetResendEmailByEmailDateRangeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResendEmail/GetResendEmailByEmailDateRange")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetResendEmailByEmailDateRange>();

            return AppResult<GetResendEmailByEmailDateRange>.CreateSucceeded(result, "Successfully getting get all resend emails api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetResendEmailByEmailDateRange>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetResendEmailByEmailDateRange>.CreateFailed(ex, "An error occured when getting all resend emails api");
        }
    }

    public async Task<AppResult<GetResendEmailResult>> GetResendEmailById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"ResendEmail/GetResendEmailById/{id}")
                            .GetJsonAsync<GetResendEmailResult>();
            
            return AppResult<GetResendEmailResult>.CreateSucceeded(result, "Successfully getting resend email by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetResendEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetResendEmailResult>.CreateFailed(ex, "An error occured when getting resend email by id api");
        }
    }

    public async Task<AppResult<UpdateEmailResendResult>> UpdateEmailResend(UpdateResendEmailArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResendEmail/UpdateEmailResend")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateEmailResendResult>();
            
            return AppResult<UpdateEmailResendResult>.CreateSucceeded(result, "Successfully posting update resend email api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateEmailResendResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateEmailResendResult>.CreateFailed(ex, "An error occured when posting update resend email api");
        }
    }
}