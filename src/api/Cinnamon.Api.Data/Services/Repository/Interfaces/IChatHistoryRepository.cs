using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatHistoryRepository
{
   
    Task<AppResult<ChatHistoryDTO>> Create(int chatRoomId, int fromUserId, int toUserId, string message, bool isViewed, string fromConnectionId, string toConnectionId);
    Task<AppResult<bool>> Update(int? chatRoomId, int? fromUserId, int? toUserId, bool? isViewed);
    Task<AppResult<IEnumerable<ChatHistoryDTO>>> GetChatHistoryByChatRoomId(int? chatRoomId, int? skip, int? take);


}