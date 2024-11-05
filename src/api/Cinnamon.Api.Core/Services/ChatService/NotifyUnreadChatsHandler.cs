using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService;

public class NotifyUnreadChatsHandler : INotifyUnreadChatsHandler
{
    private readonly IChatHistoryData chatHistoryData;
    private readonly IChatUnreadNotificationHandler chatUnreadNotificationHandler;

    public NotifyUnreadChatsHandler(IChatHistoryData chatHistoryData, IChatUnreadNotificationHandler chatUnreadNotificationHandler)
    {
        this.chatHistoryData = chatHistoryData;
        this.chatUnreadNotificationHandler = chatUnreadNotificationHandler;
    }
    
    public AppResult<NotifyUnreadChatsResult> Execute(NotifyUnreadChatsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<NotifyUnreadChatsResult>> ExecuteAsync(NotifyUnreadChatsArgs args)
    {
        try
        {
            var unreadMessagesRes = await chatHistoryData.GetUnreadMessages();
            if(!unreadMessagesRes.Succeeded || unreadMessagesRes.Result is null || !unreadMessagesRes.Result.IsSuccess)
            {
                return AppResult<NotifyUnreadChatsResult>.CreateFailed(
                    new ApplicationException(unreadMessagesRes.Result?.ErrorInfo?.Message), unreadMessagesRes.Message);
            }

            var unreadMessages = unreadMessagesRes.Result.Result;
            foreach (var message in unreadMessages)
            {
                string repeated = message.Repeated ?? "first";
                DateTime currentDate = DateTime.Now;

                // Notification conditions based on time intervals
                if (repeated.Equals("first", StringComparison.CurrentCultureIgnoreCase))
                {
                    // First notification: only if message is at least 1 hour old
                    if (currentDate < message.ChatDate.AddHours(1)) continue;
                    repeated = "second"; // Set up for the next interval
                }
                else if (repeated.Equals("second", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Second notification: only if message is at least 24 hours old
                    if (currentDate < message.ChatDate.AddHours(24)) continue;
                    repeated = "third"; // Set up for the next interval
                }
                else if (repeated.Equals("third", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Third notification: only if message is at least 2 days old
                    if (currentDate < message.ChatDate.AddDays(2)) continue;
                    repeated = "done"; // Mark as done after third notification
                }
                else
                {
                    // If repeated is "done" or any other value, skip further notifications
                    continue;
                }

                // Send notification
                var notifyRes = await chatUnreadNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.ChatUnreadNotificationArgs
                {
                    Emails = new List<string> { message.CustomerEmail }
                });
                if (!notifyRes.Succeeded || notifyRes.Result is null)
                {
                    return AppResult<NotifyUnreadChatsResult>.CreateFailed(new ApplicationException(notifyRes.Message), notifyRes.Message);
                }

                // Log the notification with updated repetition status
                var createUnreadLogRes = await chatHistoryData.CreateUnreadNotification(new Framework.ApiCommand.ApiData.ChatConnection.Request.CreateUnreadNotificationArgs
                {
                    ChatDate = message.ChatDate,
                    ChatHistoryId = message.ChatHistoryId,
                    CustomerEmail = message.CustomerEmail,
                    FromUserId = message.FromUserId,
                    ToUserId = message.ToUserId,
                    Repeated = repeated
                });
            }

            return AppResult<NotifyUnreadChatsResult>.CreateSucceeded(new(), "Successfully notify unread messages");
        }
        catch (Exception ex)
        {
            return AppResult<NotifyUnreadChatsResult>.CreateFailed(ex, "An error occured when processing notify unread chat handler.");
        }
    }
}