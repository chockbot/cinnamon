using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IChatRoomData
{
    Task<AppResult<CreateChatRoomResult>> CreateChatRoom(CreateChatRoomArgs args);
    Task<AppResult<GetChatRoomsByUserIdResult>> GetChatRoomsByUserId(GetChatRoomsByUserIdArgs args);
}
