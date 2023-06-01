using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers
{
    public interface IChatApiHandler
    {
        Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args, string token);
        Task<AppResult<UpdateChatHistoryResult>> UpdateChatHistory(UpdateChatHistoryArgs args, string token);
        Task<AppResult<GetChatHistoryByChatRoomIdResult>> GetChatHistoryByChatRoomId(GetChatHistoryByChatRoomIdArgs args, string token);
        Task<AppResult<CreateChatRoomResult>> CreateChatRoom(CreateChatRoomArgs args, string token);
        Task<AppResult<GetChatRoomsByUserIdResult>> GetChatRoomsByUserId(GetChatRoomsByUserIdArgs args, string token);
    }
}
