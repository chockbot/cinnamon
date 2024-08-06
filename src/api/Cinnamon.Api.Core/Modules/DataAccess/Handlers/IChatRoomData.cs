using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IChatRoomData
{
    Task<AppResult<CreateChatRoomResult>> CreateChatRoom(CreateChatRoomArgs args);
    Task<AppResult<GetChatRoomsByUserIdResult>> GetChatRoomsByUserId(GetChatRoomsByUserIdArgs args);
    Task<AppResult<GetChatMembersByChatRoomIdResult>> GetChatMembersByChatRoomId(GetChatMembersByChatRoomIdArgs args);
    Task<AppResult<UpdateChatMemberResult>> UpdateChatMember(UpdateChatMemberArgs args);
    Task<AppResult<CreateChatConnectionResult>> CreateChatConnection(CreateChatConnectionArgs args);
    Task<AppResult<UpdateChatConnectionResult>> UpdateChatConnection(UpdateChatConnectionArgs args);
    Task<AppResult<GetChatConnectionByCustomerResult>> GetChatConnectionByCustomer(GetChatConnectionByCustomerArgs args);
    Task<AppResult<UpdateChatRoomNameResult>> UpdateChatRoomName(UpdateChatRoomNameArgs args);
}
