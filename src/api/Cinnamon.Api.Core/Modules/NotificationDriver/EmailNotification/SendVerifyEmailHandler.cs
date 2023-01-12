using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class SendVerifyEmailHandler : ISendVerifyEmailHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly SendVerifyEmailHelper helper;
    private readonly ApplicationConfig config;

    public SendVerifyEmailHandler(ApplicationConfig config,ISendMailHandler sendMailHandler)
    {
        this.config = config;
        this.sendMailHandler = sendMailHandler;
        this.helper = new SendVerifyEmailHelper();
    }

    public AppResult<SendVerifyEmailResult> Execute(SendVerifyEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SendVerifyEmailResult>.CreateFailed(ex, "An error occured during EmailVerificationHandler");
        }
    }

    public async Task<AppResult<SendVerifyEmailResult>> ExecuteAsync(SendVerifyEmailArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.VerificationLink, config.FrontendUrl);
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Email Verification",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<SendVerifyEmailResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }
            
            return AppResult<SendVerifyEmailResult>.CreateSucceeded(new SendVerifyEmailResult(), "Verification link successfully send");
        }
        catch (Exception ex)
        {
            return AppResult<SendVerifyEmailResult>.CreateFailed(ex, "An error occured during EmailVerificationHandler");
        }
    }
}