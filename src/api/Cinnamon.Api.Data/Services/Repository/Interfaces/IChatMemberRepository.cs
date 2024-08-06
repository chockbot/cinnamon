using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatMemberRepository
{
    Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatRoomsByUserId(int userId);
    Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatMembersByChatRoomId(int chatRoomId, int userId, bool hasLeft);
    Task<AppResult<bool>> UpdateChatMember(int chatRoomId, int userId, bool hasLeft);

    Task<AppResult<IEnumerable<ChatMemberDTO>>> GetChatMembers(int roomId, int userId);
}