using Cinnamon.Core.Common;
using Cinnamon.Core.Module.EmailService.Handler;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Interactors.Results;

namespace Cinnamon.Core.Module.NotificationService.Handler.WelcomeNotifiy;

public class WelcomeNotificationHandler: IWelcomeNotification 
{
    private readonly ISendMailHandler sendMailHandler;
    private readonly WelcomeNotifyHelper helper;

    public WelcomeNotificationHandler(ISendMailHandler sendMailHandler)
    {
        this.sendMailHandler = sendMailHandler;
        this.helper = new WelcomeNotifyHelper();
    }

    public AppResult<WelcomeNotificationResult> Execute(WelcomeNotification args) 
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<WelcomeNotificationResult>> ExecuteAsync(WelcomeNotification args) 
    {
        try 
        {
            var emailBody = helper.GetTemplate();
            var sendMailResponse = await sendMailHandler
                .ExecuteAsync(new EmailService.Interactors.SendMail()
                {
                    Body = emailBody,
                    From = "dexter.echalico@cinnamon.ph",
                    Recipients = new List<string> { args.Email },
                    Subject = "Welcome to Cinnamon",
                    ContentType = "html"
                });
            
            if(!sendMailResponse.Succeeded)
            {
                return AppResult<WelcomeNotificationResult>
                    .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
            }

            return AppResult<WelcomeNotificationResult>.CreateSucceeded(new WelcomeNotificationResult(), "Welcome message successfully sent");
        }
        catch (Exception ex)
        {
            return AppResult<WelcomeNotificationResult>.CreateFailed(ex, "An error occured during WelcomeNotificationHandler");
        }
    }
}