using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ResetPassword;

public class ResetPasswordData : IResetPasswordData
{
    private readonly IFlurlClient flurlClient;

    public ResetPasswordData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateResetPasswordResult>> CreateResetPassword(CreateResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResetPassword/CreateResetPassword")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateResetPasswordResult>();

            return AppResult<CreateResetPasswordResult>.CreateSucceeded(result, "Successfully posting create reset password api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateResetPasswordResult>.CreateFailed(ex, "An error occured when posting create reset password api");
        }
    }

    public async Task<AppResult<GetAllResetPasswordResult>> GetAllResetPassword(GetAllResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResetPassword/GetAllResetPassword")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllResetPasswordResult>();

            return AppResult<GetAllResetPasswordResult>.CreateSucceeded(result, "Successfully getting get all reset passwords api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllResetPasswordResult>.CreateFailed(ex, "An error occured when getting all reset passwords api");
        }
    }

    public async Task<AppResult<GetResetPasswordResult>> GetResetPasswordByGuidToken(string guid, string token)
    {
        try
        {
            var result = await flurlClient
                            .Request($"ResetPassword/GetResetPasswordByGuidToken/{guid}/{token}")
                            .GetJsonAsync<GetResetPasswordResult>();
            
            return AppResult<GetResetPasswordResult>.CreateSucceeded(result, "Successfully getting reset password by guid and token api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetResetPasswordResult>.CreateFailed(ex, "An error occured when getting reset password by guid and token api");
        }
    }

    public async Task<AppResult<GetResetPasswordResult>> GetResetPasswordById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"ResetPassword/GetResetPasswordById/{id}")
                            .GetJsonAsync<GetResetPasswordResult>();
            
            return AppResult<GetResetPasswordResult>.CreateSucceeded(result, "Successfully getting reset password by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetResetPasswordResult>.CreateFailed(ex, "An error occured when getting reset password by id api");
        }
    }

    public async Task<AppResult<UpdateResetPasswordResult>> UpdateResetPassword(UpdateResetPasswordArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("ResetPassword/UpdateResetPassword")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateResetPasswordResult>();
            
            return AppResult<UpdateResetPasswordResult>.CreateSucceeded(result, "Successfully posting update reset password api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateResetPasswordResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateResetPasswordResult>.CreateFailed(ex, "An error occured when posting update reset password api");
        }
    }
}