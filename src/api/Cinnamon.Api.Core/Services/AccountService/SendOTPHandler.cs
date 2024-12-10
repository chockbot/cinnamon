using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class SendOTPHandler : ISendOTPHandler
{
    private readonly ISendEmailOTPHandler sendEmailOTPHandler;
    private readonly IGuestOTPData guestOTPData;

    public SendOTPHandler(ISendEmailOTPHandler sendEmailOTPHandler, IGuestOTPData guestOTPData)
    {
        this.sendEmailOTPHandler = sendEmailOTPHandler;
        this.guestOTPData        = guestOTPData;
    }

    public AppResult<SendOTPResult> Execute(SendOTPArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SendOTPResult>.CreateFailed(ex, "An error occured in SendOTPHandler");
        }
    }

    public async Task<AppResult<SendOTPResult>> ExecuteAsync(SendOTPArgs args)
    {
        try
        {
            if (string.IsNullOrEmpty(args.Email))
            {
                return AppResult<SendOTPResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }
            //Send otp via email
            string otpCommaSeparated = string.Join(",", args.OTPCode);

            var sendOTPEmail = await sendEmailOTPHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.SendEmailOTPArgs
            {
                Email = args.Email,
                OTPCode = args.OTPCode,
            });
            if (!sendOTPEmail.Succeeded || sendOTPEmail.Result == null)
            {
                return AppResult<SendOTPResult>.CreateFailed(
                    new ApplicationException("An error occured when sending email verification"), "An error occured when sending email verification");
            }
            //Store OTP Code
            var storeOTP = await guestOTPData.CreateOTP(new Framework.ApiCommand.ApiData.GuestOTP.Request.CreateGuestOTPArgs
            {
                Email = args.Email,
                OTECode = otpCommaSeparated
            });

            if (!storeOTP.Succeeded || storeOTP.Result == null)
            {
                return AppResult<SendOTPResult>.CreateFailed(
                    new ApplicationException("An error occured when storing otp verification"), "An error occured when storing otp verification");
            }

            return AppResult<SendOTPResult>.CreateSucceeded(new SendOTPResult{
                    Email = args.Email,
                    OTPCode = args.OTPCode
            }, "Successfully send otp email");
        }
        catch (Exception ex)
        {
            return AppResult<SendOTPResult>.CreateFailed(ex, "An error occured in SendOTPHandler");
        }
    }
}
