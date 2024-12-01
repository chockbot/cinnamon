using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class GetUserOTPHandler : IGetUserOTPHandler
{
    private readonly IGuestOTPData guestOTPData;
    public GetUserOTPHandler(IGuestOTPData guestOTPData)
    {
        this.guestOTPData = guestOTPData;
    }

    public AppResult<GetUserOTPResult> Execute(GetUserOTPArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetUserOTPResult>.CreateFailed(ex, "An error occured in GetUserOTPHandler");
        }
    }

    public async Task<AppResult<GetUserOTPResult>> ExecuteAsync(GetUserOTPArgs args)
    {
        try
        {
            var result = await guestOTPData.GetOTPByEmail(new Framework.ApiCommand.ApiData.GuestOTP.Request.GetGuestOTPByEmailArgs
            {
                Email = args.Email
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetUserOTPResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetUserOTPResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetUserOTPHandler");
            }

            var otpResult = result.Result;

            return AppResult<GetUserOTPResult>.CreateSucceeded(new GetUserOTPResult
            {
                GuestOTPs = result.Result.Result.Select(a => new GetUserOTPResult.GuestOTP
                {
                    Email     = a.Email,
                    OTPCode   = a.OTPCode,
                    CreatedOn = a.CreatedOn
                })
            }, "successfully retrieved otps");
        }
        catch (Exception ex)
        {
            return AppResult<GetUserOTPResult>.CreateFailed(ex, "An error occured in GetUserOTPHandler");
        }
    }
}
