using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class SendEmailOTPHandler : ISendEmailOTPHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly SendEmailOTPHelper helper;
    private readonly ApplicationConfig config;

    public SendEmailOTPHandler(ApplicationConfig config, ISendMailHandler sendMailHandler)
    {
        this.config = config;
        this.sendMailHandler = sendMailHandler;
        this.helper = new SendEmailOTPHelper();
    }

    public AppResult<SendEmailOTPResult> Execute(SendEmailOTPArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SendEmailOTPResult>.CreateFailed(ex, "An error occured during SendOTPHandler");
        }
    }

    public async Task<AppResult<SendEmailOTPResult>> ExecuteAsync(SendEmailOTPArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.OTPCode, config.FrontendUrl);
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs
                {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Use OTP to Verify Your Identity",
                    ContentType = "html"
                });

            if (!sendMailResponse.Succeeded)
            {
                return AppResult<SendEmailOTPResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<SendEmailOTPResult>.CreateSucceeded(new SendEmailOTPResult(), "OTP code successfully send");
        }
        catch (Exception ex)
        {
            return AppResult<SendEmailOTPResult>.CreateFailed(ex, "An error occured during SendOTPHandler");
        }
    }
}
