using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IChatHistoryData
{
    Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args);
    Task<AppResult<UpdateChatHistoryResult>> UpdateChatHistory(UpdateChatHistoryArgs args);
    Task<AppResult<GetChatHistoryByChatRoomIdResult>> GetChatHistoryByChatRoomId(GetChatHistoryByChatRoomIdArgs args);
    Task<AppResult<GetUnreadMessagesResult>> GetUnreadMessages();
    Task<AppResult<CreateUnreadNotificationResult>> CreateUnreadNotification(CreateUnreadNotificationArgs args);
}
