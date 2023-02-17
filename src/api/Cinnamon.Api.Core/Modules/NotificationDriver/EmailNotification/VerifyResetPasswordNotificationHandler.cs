using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class VerifyResetPasswordNotificationHandler : IVerifyResetPasswordNotificationHandler
{
    private readonly VerifyResetPasswordNotificationHelper helper;
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;

    public VerifyResetPasswordNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.helper = new();
        this.sendMailHandler = sendMailHandler;
        this.config = config;
    }

    public AppResult<VerifyResetPasswordNotificationResult> Execute(VerifyResetPasswordNotificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyResetPasswordNotificationResult>.CreateFailed(ex, "An error occured in VerifyResetPasswordNotificationHandler");
        }
    }

    public async Task<AppResult<VerifyResetPasswordNotificationResult>> ExecuteAsync(VerifyResetPasswordNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.Email, args.DateChanged, config.FrontendUrl);
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Successfully Resetted Password",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<VerifyResetPasswordNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<VerifyResetPasswordNotificationResult>.CreateSucceeded(new VerifyResetPasswordNotificationResult(), "Verification link successfully send");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyResetPasswordNotificationResult>.CreateFailed(ex, "An error occured in VerifyResetPasswordNotificationHandler");
        }
    }
}