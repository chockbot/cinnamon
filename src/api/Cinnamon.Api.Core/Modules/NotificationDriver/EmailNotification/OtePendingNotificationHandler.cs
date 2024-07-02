using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class OtePendingNotificationHandler : IOtePendingNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly OtePendingNotificationHelper helper;

    public OtePendingNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new();
    }

    public AppResult<OtePendingNotificationResult> Execute(OtePendingNotificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OtePendingNotificationResult>> ExecuteAsync(OtePendingNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.EventName, args.Body, args.CustomerName);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.CustomerEmail },
                    Subject = "Event Pending for Approval",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<OtePendingNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<OtePendingNotificationResult>.CreateSucceeded(new(), "Event norification sent.");
        }
        catch (Exception ex)
        {
            return AppResult<OtePendingNotificationResult>.CreateFailed(ex, "An error occured in OtePendingNotificationHandler.");
        }
    }
}