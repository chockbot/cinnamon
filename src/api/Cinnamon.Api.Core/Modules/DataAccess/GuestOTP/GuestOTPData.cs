using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;
using Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.GuestOTP;
public class GuestOTPData : IGuestOTPData
{
    private readonly IFlurlClient flurlClient;
    public GuestOTPData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        this.flurlClient = flurlFac.Get(config.ApiDataUrl);
    }
    public async Task<AppResult<CreateGuestOTPResult>> CreateOTP(CreateGuestOTPArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OTP/CreateOTP")
                .PostJsonAsync(args)
                .ReceiveJson<CreateGuestOTPResult>();

            return AppResult<CreateGuestOTPResult>.CreateSucceeded(result, "Successfully posted create otp api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateGuestOTPResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateGuestOTPResult>.CreateFailed(ex, "An error occured when posting create otp api");
        }
    }

    public async Task<AppResult<GetGuestOTPByEmailResult>> GetOTPByEmail(GetGuestOTPByEmailArgs args)
    {
        try
        {
            var result = await flurlClient
                        .Request("OTP/GetOTPByEmail")
                        .SetQueryParams(args)
                        .GetJsonAsync<GetGuestOTPByEmailResult>();

            return AppResult<GetGuestOTPByEmailResult>.CreateSucceeded(result, "Successfully posted get otp api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetGuestOTPByEmailResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetGuestOTPByEmailResult>.CreateFailed(ex, "An error occured when posting get otp api");
        }
    }
}
