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

            var currentDate = DateTime.Now.AddHours(-1);
            var unreadMessageInHour = unreadMessagesRes.Result.Result.Where(m => currentDate > m.ChatDate);

            for(int i =0; i < unreadMessageInHour.Count(); i++)
            {
                var message = unreadMessageInHour.ElementAt(i);

                var notifyRes = await chatUnreadNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.ChatUnreadNotificationArgs {
                    Emails = new List<string> {message.CustomerEmail}
                });
                if(!notifyRes.Succeeded || notifyRes.Result is null)
                {
                    return AppResult<NotifyUnreadChatsResult>.CreateFailed(new ApplicationException(notifyRes.Message), notifyRes.Message);
                }

                var createUnreadLogRes = await chatHistoryData.CreateUnreadNotification(new Framework.ApiCommand.ApiData.ChatConnection.Request.CreateUnreadNotificationArgs {
                    ChatDate = message.ChatDate,
                    ChatHistoryId = message.ChatHistoryId,
                    CustomerEmail = message.CustomerEmail,
                    FromUserId = message.FromUserId,
                    ToUserId = message.ToUserId
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