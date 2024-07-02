using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class OteEventReminderNotificationHandler : IOteEventReminderNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly OteEventReminderNotificationHelper helper;
    private readonly ApplicationConfig config;

    public OteEventReminderNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.config = config;
        this.sendMailHandler = sendMailHandler;
        this.helper = new();
    }
    
    public AppResult<OteEventReminderNotificationResult> Execute(OteEventReminderNotificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteEventReminderNotificationResult>> ExecuteAsync(OteEventReminderNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.EventName, args.Subject, 
                args.Body, args.CustomerName, args.EventDate, args.EventLocation, args.DaysStart);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.CustomerEmail },
                    Subject = "Event Reminder",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<OteEventReminderNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<OteEventReminderNotificationResult>.CreateSucceeded(new(), "Event Reminder sent.");
        }
        catch (Exception ex)
        {
            return AppResult<OteEventReminderNotificationResult>.CreateFailed(ex, "An error occured in OteEventReminderNotificationHandler.");
        }
    }
}