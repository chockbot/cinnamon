using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatRoomRepository
{
   
    Task<AppResult<ChatRoomDTO>> Create(int fromUserId, int toUserId, Enums.ChatType chatType, string groupName, string chatName);

    Task<AppResult<ChatRoomDTO>> UpdateChatRoomName(int chatRoomId, string newChatRoomName);
}