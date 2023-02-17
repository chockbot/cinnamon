using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class ResetPasswordNotificationHandler : IResetPasswordNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly ResetPasswordNotificationHelper resetPasswordNotificationHelper;

    public ResetPasswordNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.resetPasswordNotificationHelper = new();
    }

    public AppResult<ResetPasswordNotificationResult> Execute(ResetPasswordNotificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordNotificationResult>.CreateFailed(ex, "An error occured in ResetPasswordNotificationHandler");
        }
    }

    public async Task<AppResult<ResetPasswordNotificationResult>> ExecuteAsync(ResetPasswordNotificationArgs args)
    {
        try
        {
            var emailBody = resetPasswordNotificationHelper.GetTemplate(args.VerificationLink, config.FrontendUrl, args.Email);
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Reset Password",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<ResetPasswordNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }
            
            return AppResult<ResetPasswordNotificationResult>.CreateSucceeded(new ResetPasswordNotificationResult(), "Reset Password link successfully send");
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordNotificationResult>.CreateFailed(ex, "An error occured in ResetPasswordNotificationHandler");
        }
    }
}