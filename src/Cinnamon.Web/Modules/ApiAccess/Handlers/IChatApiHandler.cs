using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Response;
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
        Task<AppResult<UpdateConnectionIdResult>> UpdateConnectionId(UpdateConnectionIdArgs args, string token);
        Task<AppResult<GetChatMembersByChatRoomIdResult>> GetChatMembersByChatRoomId(GetChatMembersByChatRoomIdArgs args, string token);
        Task<AppResult<GetChatConnectionByCustomerResult>> GetChatConnectionByCustomer(GetChatConnectionByCustomerArgs args, string token);
        Task<AppResult<RequestMessageResult>> RequestMessage(RequestMessageArgs args, string token);
        Task<AppResult<GetRequestMessageResult>> GetRequestMessage(GetRequestMessageArgs args, string token);

        Task<AppResult<UpdateChatRoomNameResult>> UpdateChatRoomName(UpdateChatRoomNameArgs args, string token);
    }
}
