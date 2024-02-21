using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification.Helpers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.EmailNotification;

public class ChatUnreadNotificationHandler : IChatUnreadNotificationHandler
{
    private readonly ChatUnreadNotificationHelper helper = new();
    private readonly ISendMailHandler sendMailHandler;
    private readonly ApplicationConfig config;

    public ChatUnreadNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig config)
    {
        this.sendMailHandler = sendMailHandler;
        this.config = config;
    }

    public AppResult<ChatUnreadNotificationResult> Execute(ChatUnreadNotificationArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ChatUnreadNotificationResult>> ExecuteAsync(ChatUnreadNotificationArgs args)
    {
        try
        {
            var emailBody = helper.GetTemplate(config.FrontendUrl);

            var sendMailResponse = await sendMailHandler.ExecuteAsync(new EmailDriver.Interactors.SendMailArgs {
                Body = emailBody,
                Recipients = args.Emails,
                ContentType = "html",
                Subject = "Unread Messages"
            });
            if(!sendMailResponse.Succeeded || sendMailResponse.Result is null)
            {
                return AppResult<ChatUnreadNotificationResult>.CreateFailed(new ApplicationException(sendMailResponse.Message), sendMailResponse.Message);
            }

            return AppResult<ChatUnreadNotificationResult>.CreateSucceeded(new(), "Successfully send notification.");
        }
        catch (Exception ex)
        {
            return AppResult<ChatUnreadNotificationResult>.CreateFailed(ex, "An error occured when sending notification.");
        }
    }
}