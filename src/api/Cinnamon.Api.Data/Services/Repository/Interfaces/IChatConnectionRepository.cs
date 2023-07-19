using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Waitlist;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;
using Cinnamon.Framework.Enums;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatConnection;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatConnectionRepository
{
   
    Task<AppResult<bool>> Create(int customerId, string connectionId, string userAgent, bool isConnected);
    Task<AppResult<bool>> Update(int customerId, string connectionId, bool isConnected);
    Task<AppResult<IEnumerable<ChatConnectionDTO>>> GetChatConnectionByCustomer(int customerId);
}