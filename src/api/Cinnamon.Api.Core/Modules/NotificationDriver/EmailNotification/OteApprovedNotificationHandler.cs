using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class OteApprovedNotificationHandler : IOteApprovedNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly OteApprovedNotificationHelper helper;

    public OteApprovedNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new();
    }
    
    public AppResult<OteApprovedNotificationResult> Execute(OteApprovedNotificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteApprovedNotificationResult>> ExecuteAsync(OteApprovedNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.EventName, args.Body, args.CustomerName, 
                args.EventDate, args.EventLocation, args.ApprovedLink, args.PaidTicket);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.CustomerEmail },
                    Subject = "Event Approved",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<OteApprovedNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<OteApprovedNotificationResult>.CreateSucceeded(new(), "Event norification sent.");
        }
        catch (Exception ex)
        {
            return AppResult<OteApprovedNotificationResult>.CreateFailed(ex, "An error occured in OteApprovedNotificationHandler.");
        }
    }
}