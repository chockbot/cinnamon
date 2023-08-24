using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class ExpiringStudentNotificationHandler : IExpiringStudentNotificationHandler
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly ExpiringStudentNotificationHelper helper;

    public ExpiringStudentNotificationHandler(ISendMailHandler sendMailHandler)
    {
        this.sendMailHandler = sendMailHandler;
        this.helper = new();
    }

    public AppResult<ExpiringStudentNotificationResult> Execute(ExpiringStudentNotificationArgs interactor)
    {
        try
        {
            return ExecuteAsync(interactor).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ExpiringStudentNotificationResult>.CreateFailed(ex, "An error occured in ExpiringStudentNotificationHandler");
        }
    }

    public async Task<AppResult<ExpiringStudentNotificationResult>> ExecuteAsync(ExpiringStudentNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(args.DateSend, args.FirstName, args.LastName, args.ActivityTitle,
               args.ExpiredDate, args.Amount, args.ActivityLink, args.Address, args.ImageLocation, 
               args.APrice, args.Rating, args.Count);
            
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                    Body = emailBody,
                    Recipients = new List<string> { args.Email },
                    Subject = "Expiring Activity",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<ExpiringStudentNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<ExpiringStudentNotificationResult>.CreateSucceeded(
                    new ExpiringStudentNotificationResult(), "Expiring activity notification successfully sent.");
        }
        catch (Exception ex)
        {
             return AppResult<ExpiringStudentNotificationResult>.CreateFailed(ex, "An error occured in ExpiringStudentNotificationHandler");
        }
    }
}