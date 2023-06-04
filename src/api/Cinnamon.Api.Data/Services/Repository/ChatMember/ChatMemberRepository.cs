using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Barangay;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatRoom;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.City;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Region;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ChatRoom
{
    public class ChatMemberRepository : IChatMemberRepository
    {
        private readonly IDataStore dataStore;

        public ChatMemberRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatRoomsByUserId(int userId)
        {
            try
            {
                var customerChatRoomsResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == userId);

                if (!customerChatRoomsResult.Succeeded || customerChatRoomsResult.Result == null)
                {
                    return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(customerChatRoomsResult.Error.Exception, customerChatRoomsResult.Message);
                }

                Expression<Func<Entities.ChatMember, bool>> filter =
                a => (customerChatRoomsResult.Result.Select(c => c.ChatRoomId).Contains(a.ChatRoomId));

                var includes = new List<Expression<Func<Entities.ChatMember, object>>>
                {
                    a => a.Customer,
                    a => a.ChatRoom
                };

                List<ChatRoomDTO> chatRooms = new List<ChatRoomDTO>();

                var result = await dataStore.ChatMember.GetChatRoomsByUserId(filter, includes);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var chatRoomIds = result.Result.GroupBy(c => c.ChatRoomId).Select(c => c.Key);

                foreach (var chatRoomId in chatRoomIds)
                {
                    var fromCustomer = result.Result.Where(c => c.ChatRoomId == chatRoomId && c.CustomerId != userId).FirstOrDefault()?.Customer;
                    var toCustomer = result.Result.Where(c => c.ChatRoomId == chatRoomId && c.CustomerId == userId).FirstOrDefault()?.Customer;
                    var chatDetail = result.Result.Where(c => c.ChatRoomId == chatRoomId && c.CustomerId == userId).FirstOrDefault();
                    var chatHistoryResult = await dataStore.ChatHistory.FindAsync(c => c.ChatRoomId == chatRoomId && c.FromUserId == fromCustomer.Id && c.ToUserId == toCustomer.Id && !c.IsViewed);
                    
                    if (!chatHistoryResult.Succeeded || chatHistoryResult.Result == null)
                    {
                        return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(chatHistoryResult.Error.Exception, chatHistoryResult.Message);
                    }

                    var chatHistory = chatHistoryResult.Result;

                    if (fromCustomer != null && toCustomer != null && chatDetail != null)
                    {
                        chatRooms.Add(new ChatRoomDTO
                        {
                            ChatRoomId       = chatRoomId,
                            DateCreated      = chatDetail.CreatedOn,
                            FromFirstName    = fromCustomer.FirstName,
                            FromLastName     = fromCustomer.LastName,
                            FromProfilePath  = fromCustomer.ProfilePath,
                            FromUserId       = fromCustomer.Id,
                            Message          = chatDetail.ChatRoom.LatestMessage,
                            ToFirstName      = toCustomer.FirstName,
                            ToLastName       = toCustomer.LastName,
                            ToProfilePath    = toCustomer.ProfilePath,
                            ToUserId         = toCustomer.Id,
                            FromConnectionId = fromCustomer.ConnectionId,
                            ToConnectionId   = toCustomer.ConnectionId,
                            HasNewMessage    = chatHistory.Any()
                        });
                    }
                }

                return AppResult<IEnumerable<ChatRoomDTO>>.CreateSucceeded(chatRooms, "Successfully retrieved chat room");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(ex, "An error occured when retrieving chat room");
            }
        }
    }
}
