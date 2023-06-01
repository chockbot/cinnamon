using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatRoomRepository
{
   
    Task<AppResult<ChatRoomDTO>> Create(int fromUserId, int toUserId);
    Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatRoomsByUserId(int userId);
}