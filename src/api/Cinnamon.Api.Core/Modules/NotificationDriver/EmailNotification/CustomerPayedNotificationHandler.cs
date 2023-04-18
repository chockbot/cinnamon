using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class CustomerPayedNotificationHandler : ICustomerPayedNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;
    private readonly CustomerPayedNotificationHelper helper;

    public CustomerPayedNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
        this.helper = new();
    }

    public AppResult<CustomerPayedNotificationResult> Execute(CustomerPayedNotificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPayedNotificationResult>.CreateFailed(ex, "An error occured during EmailVerificationHandler");
        }
    }

    public async Task<AppResult<CustomerPayedNotificationResult>> ExecuteAsync(CustomerPayedNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.CustomerName, args.ExperienceName, 
                args.CoachName, args.PurchaseDate, args.PayerName, args.Amount, 
                config.FrontendUrl, args.Members, args.ReferenceNumber, args.PaymentMethod, args.MakerEmail);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Payment Confirmation",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<CustomerPayedNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<CustomerPayedNotificationResult>.CreateSucceeded(
                    new CustomerPayedNotificationResult(), "Payment confirmation successfully sent");
        }
        catch (Exception ex)
        {
            return AppResult<CustomerPayedNotificationResult>.CreateFailed(ex, "An error occured in CustomerPayedNotificationHandler");
        }
    }
}