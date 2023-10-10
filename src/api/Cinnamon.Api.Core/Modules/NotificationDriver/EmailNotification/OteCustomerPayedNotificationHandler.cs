using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class OteCustomerPayedNotificationHandler : IOteCustomerPayedNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly OtePurchaseVerification helper;

    public OteCustomerPayedNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new();
    }

    public AppResult<OteCustomerPayedNotificationResult> Execute(OteCustomerPayedNotificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteCustomerPayedNotificationResult>.CreateFailed(ex, "An error occured in OteCustomerPayedNotificationHandler");
        }
    }

    public async Task<AppResult<OteCustomerPayedNotificationResult>> ExecuteAsync(OteCustomerPayedNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate();

            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Payment Confirmation",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<OteCustomerPayedNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<OteCustomerPayedNotificationResult>.CreateSucceeded(
                    new OteCustomerPayedNotificationResult(), "Payment confirmation successfully sent");

        }
        catch (Exception ex)
        {
            return AppResult<OteCustomerPayedNotificationResult>.CreateFailed(ex, "An error occured in OteCustomerPayedNotificationHandler");
        }
    }
}