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
using Cinnamon.Framework.Enums;
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

        public async Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatMembersByChatRoomId(int chatRoomId, int userId, bool hasLeft)
        {
            Expression<Func<Entities.ChatMember, bool>> filter = a => a.ChatRoomId == chatRoomId && a.HasLeft == hasLeft;

            var includes = new List<Expression<Func<Entities.ChatMember, object>>>
                {
                    a => a.Customer,
                    a => a.ChatRoom
                };

            List<ChatRoomDTO> chatMembers = new List<ChatRoomDTO>();

            var commonPrivateChatRoomId = 0;

            var result = await dataStore.ChatMember.FindAsync(filter, int.MaxValue, 0, includes);

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            foreach (var item in result.Result)
            {
                var chatConnectionresult = await dataStore.ChatConnection.FindAsync(c => c.CustomerId == item.Customer.Id && c.IsConnected, int.MaxValue, 0);

                if (!chatConnectionresult.Succeeded || chatConnectionresult.Result == null)
                {
                    return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                chatMembers.Add(new ChatRoomDTO
                {
                    FromUserId = item.Customer.Id,
                    FromFirstName = item.Customer.FirstName,
                    FromLastName = item.Customer.LastName,
                    FromProfilePath = item.Customer.ProfilePath,
                    FromProfileLink = item.Customer.Handler,
                    ChatMemberType = (Enums.ChatMemberType)item.ChatMemberType,
                    ConnectionIds = chatConnectionresult.Result.Select(c => c.ConnectionId)
                });
            }

            return AppResult<IEnumerable<ChatRoomDTO>>.CreateSucceeded(chatMembers, "Successfully retrieved chat members");
        }

        public async Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatRoomsByUserId(int userId)
        {
            try
            {
                string profilePicture = string.Empty;

                var customerChatRoomsResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == userId && !c.HasLeft);

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
                    var newMessageResult = await dataStore.ChatHistory.FindAsync(c => c.ChatRoomId == chatRoomId && c.ToUserId == toCustomer.Id && !c.IsViewed,1,0);
                    var oldMessageResult = await dataStore.ChatHistory.GetOrderedChatHistoryByChatRoomId(c => c.ChatRoomId == chatRoomId && (c.FromUserId == userId || c.ToUserId == userId),1,0);
                    
                    if (!newMessageResult.Succeeded || newMessageResult.Result == null)
                    {
                        return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(newMessageResult.Error.Exception, newMessageResult.Message);
                    }

                    if (!oldMessageResult.Succeeded || oldMessageResult.Result == null)
                    {
                        return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(oldMessageResult.Error.Exception, oldMessageResult.Message);
                    }

                    if ((Enums.ChatType)chatDetail.ChatRoom.ChatType == Enums.ChatType.GroupChat)
                    {
                        profilePicture = result.Result.FirstOrDefault(c => c.ChatRoomId == chatRoomId && (Enums.ChatMemberType)c.ChatMemberType == Enums.ChatMemberType.ActivityProvider)?.Customer?.ProfilePath;
                    }
                    else
                    {
                        profilePicture = fromCustomer.ProfilePath;
                    }

                    var newChatHistory = newMessageResult.Result;
                    var oldChatHistory = oldMessageResult.Result;

                    if (fromCustomer != null && toCustomer != null && chatDetail != null)
                    {
                        chatRooms.Add(new ChatRoomDTO
                        {
                            ChatRoomId       = chatRoomId,
                            DateCreated      = oldChatHistory.Any() ? oldChatHistory.FirstOrDefault().CreatedOn : chatDetail.CreatedOn,
                            FromFirstName    = fromCustomer.FirstName,
                            FromLastName     = fromCustomer.LastName,
                            FromProfilePath  = profilePicture,
                            FromUserId       = fromCustomer.Id,
                            FromProfileLink  = fromCustomer.Handler,
                            Message          = chatDetail.ChatRoom.LatestMessage,
                            ToFirstName      = toCustomer.FirstName,
                            ToLastName       = toCustomer.LastName,
                            ToProfilePath    = toCustomer.ProfilePath,
                            ToUserId         = toCustomer.Id,
                            FromConnectionId = fromCustomer.ConnectionId,
                            ToConnectionId   = toCustomer.ConnectionId,
                            HasNewMessage    = newChatHistory.Any(),
                            ChatName         = chatDetail.ChatRoom.Name,
                            ChatType         = (Enums.ChatType)chatDetail.ChatRoom.ChatType,
                            GroupName        = chatDetail.ChatRoom.GroupName,
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

        public async Task<AppResult<bool>> UpdateChatMember(int chatRoomId, int userId, bool hasLeft)
        {
            try
            {
                Expression<Func<Entities.ChatMember, bool>> filter =
                a => (a.ChatRoomId == chatRoomId && a.CustomerId == userId && !a.HasLeft);

                var result = await dataStore.ChatMember.FindFirstAsync(filter);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
                }

                result.Result.HasLeft = hasLeft;
                result.Result.ChangedBy = userId;
                result.Result.ChangedOn = DateTime.UtcNow;

                var updatedRes = await dataStore.ChatMember.Update(result.Result);

                if (!updatedRes.Succeeded || updatedRes.Result == null)
                {
                    return AppResult<bool>.CreateFailed(
                        new ApplicationException("An error occured when updating all entities"), "An error occured when updating all entities");
                }

                return AppResult<bool>.CreateSucceeded(true, "Successfully updated chat member");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured when updating all entities");
            }
        }

        public async Task<AppResult<IEnumerable<ChatMemberDTO>>> GetChatMembers(int roomId, int userId)
        {
            try
            {
                var chatMembers = await dataStore.ChatMember.FindAsync(c => c.ChatRoomId == roomId && c.CustomerId == userId);
                if(!chatMembers.Succeeded || chatMembers.Result == null)
                {
                    return AppResult<IEnumerable<ChatMemberDTO>>.CreateFailed(chatMembers.Error.Exception, chatMembers.Message);
                }

                return AppResult<IEnumerable<ChatMemberDTO>>.CreateSucceeded(chatMembers.Result.Select(c => new ChatMemberDTO
                {
                    Id = c.Id,
                    ChatRoomId = c.ChatRoomId,
                    CustomerId = c.CustomerId,
                    HasLeft = c.HasLeft,
                    ChatMemberType = c.ChatMemberType
                }), "Successfully retrieved chat members");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatMemberDTO>>.CreateFailed(ex, "An error occured when retrieving chat members");
            }
        }
    }
}
