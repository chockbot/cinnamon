using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class OteThankYouNotificationHandler : IOteThankYouNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly OteThankYouNotificationHelper helper;

    public OteThankYouNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new();
    }

    public AppResult<OteThankYouNotificationResult> Execute(OteThankYouNotificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteThankYouNotificationResult>> ExecuteAsync(OteThankYouNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.EventName, args.Subject, args.Body, args.CustomerName);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.CustomerEmail },
                    Subject = "Thank you",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<OteThankYouNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<OteThankYouNotificationResult>.CreateSucceeded(new(), "Thank you email send.");
        }
        catch (Exception ex)
        {
            return AppResult<OteThankYouNotificationResult>.CreateFailed(ex, "An error occured in OteThankYouNotificationHandler.");
        }
    }
}