using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Barangay;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.City;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Region;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ChatHistory
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private readonly IDataStore dataStore;

        public ChatHistoryRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<AppResult<ChatHistoryDTO>> Create(int chatRoomId, int fromUserId, int toUserId, string message, bool isViewed, string fromConnectionId, string toConnectionId)
        {
            try
            {
                List<Entities.ChatHistory> chatHistories = new List<Entities.ChatHistory>();

                var includes = new List<Expression<Func<Entities.ChatMember, object>>>
                {
                    a => a.Customer,
                };

                var chatMemberResult = await dataStore.ChatMember.FindAsync(c => c.ChatRoomId == chatRoomId && c.CustomerId != fromUserId, 1000, 0, includes);

                if (!chatMemberResult.Succeeded || chatMemberResult.Result == null)
                {
                    return AppResult<ChatHistoryDTO>.CreateFailed(chatMemberResult.Error.Exception, chatMemberResult.Message);
                }

                var customerResult = await dataStore.Customer.FindFirstAsync(c => c.Id == fromUserId);

                if (!customerResult.Succeeded || customerResult.Result == null)
                {
                    return AppResult<ChatHistoryDTO>.CreateFailed(customerResult.Error.Exception, customerResult.Message);
                }

                chatHistories.AddRange(chatMemberResult.Result.Select(c => 
                    new Entities.ChatHistory {
                        ChatRoomId       = chatRoomId,
                        FromUserId       = fromUserId,
                        ToUserId         = c.CustomerId,
                        Message          = message,
                        IsViewed         = isViewed,
                        FromConnectionId = customerResult.Result.ConnectionId,
                        ToConnectionId   = string.IsNullOrEmpty(c.Customer.ConnectionId) ? string.Empty : c.Customer.ConnectionId,
                        CreatedOn        = DateTime.UtcNow
                    })
                );

                var result = await dataStore.ChatHistory.AddRange(chatHistories);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<ChatHistoryDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                var created = result.Result.FirstOrDefault();

                var chatRoomResult = await dataStore.ChatRooms.FindFirstAsync(c => c.Id == chatRoomId);

                if (!chatRoomResult.Succeeded || chatRoomResult.Result == null)
                {
                    return AppResult<ChatHistoryDTO>.CreateFailed(chatRoomResult.Error.Exception, chatRoomResult.Message);
                }

                chatRoomResult.Result.LatestMessage = message;

                var chatRoomUpdateResult = await dataStore.ChatRooms.Update(chatRoomResult.Result);

                return AppResult<ChatHistoryDTO>.CreateSucceeded(new ChatHistoryDTO
                {
                   ChatRoomId       = created.ChatRoomId,
                   FromUserId       = created.FromUserId,
                   ToUserId         = created.ToUserId,
                   Message          = created.Message,
                   IsViewed         = created.IsViewed,
                   FromConnectionId = created.FromConnectionId,
                   ToConnectionId   = created.ToConnectionId
                }, "Successully created chat history");
            }
            catch (Exception ex)
            {
                return AppResult<ChatHistoryDTO>.CreateFailed(ex, "An error occured when creating chat history");
            }
        }

        public async Task<AppResult<IEnumerable<ChatHistoryDTO>>> GetChatHistoryByChatRoomId(int? chatRoomId, int? skip, int? take)
        {
            try
            {
                Expression<Func<Entities.ChatHistory, bool>> filter =
                a => ((chatRoomId.HasValue ? a.ChatRoomId == chatRoomId.Value : true));

                var includes = new List<Expression<Func<Entities.ChatHistory, object>>>
                {
                    a => a.FromCustomer,
                    a => a.ToCustomer
                };

                var result = await dataStore.ChatHistory.GetOrderedChatHistoryByChatRoomId(filter, take, skip, includes);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ChatHistoryDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var chatHistories = result.Result.Select(c =>
                {
                    var chatHistoryDto = new ChatHistoryDTO
                    {
                        ChatHistoryId = c.Id,
                        ChatRoomId    = c.ChatRoomId,
                        FromUserId    = c.FromUserId,
                        ToUserId      = c.ToUserId,
                        DateCreated   = c.CreatedOn,
                        IsViewed      = c.IsViewed,
                        Message       = c.Message
                    };

                    if (c.FromCustomer != null)
                    {
                        chatHistoryDto.FromUserId      = c.FromCustomer.Id;
                        chatHistoryDto.FromLastName    = c.FromCustomer.LastName;
                        chatHistoryDto.FromFirstName   = c.FromCustomer.FirstName;
                        chatHistoryDto.FromProfilePath = c.FromCustomer.ProfilePath;
                    }

                    if (c.ToCustomer != null)
                    {
                        chatHistoryDto.ToUserId      = c.ToCustomer.Id;
                        chatHistoryDto.ToLastName    = c.ToCustomer.LastName;
                        chatHistoryDto.ToFirstName   = c.ToCustomer.FirstName;
                        chatHistoryDto.ToProfilePath = c.ToCustomer.ProfilePath;
                    }

                    return chatHistoryDto;
                });

                return AppResult<IEnumerable<ChatHistoryDTO>>.CreateSucceeded(chatHistories, "Successfully retrieved chat histories");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatHistoryDTO>>.CreateFailed(ex, "An error occured when retrieving chat histories");
            }
        }

        public async Task<AppResult<bool>> Update(int? chatRoomId, int? fromUserId, int? toUserId, bool? isViewed)
        {
            try
            {
                Expression<Func<Entities.ChatHistory, bool>> filter =
                a => ((chatRoomId.HasValue ? a.ChatRoomId == chatRoomId.Value : true) &&
                      (fromUserId.HasValue ? a.FromUserId == fromUserId.Value : true) &&
                      (toUserId.HasValue ? a.ToUserId == toUserId.Value : true));

                var result = await dataStore.ChatHistory.FindAsync(filter);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
                }

                foreach (var item in result.Result)
                {
                    item.IsViewed = isViewed.GetValueOrDefault();
                    item.ChangedBy = toUserId.Value;
                    item.ChangedOn = DateTime.UtcNow;
                }

                var updatedRes = await dataStore.ChatHistory.UpdateRange(result.Result);

                if (!updatedRes.Succeeded || updatedRes.Result == null)
                {
                    return AppResult<bool>.CreateFailed(
                        new ApplicationException("An error occured when updating all entities"), "An error occured when updating all entities");
                }

                return AppResult<bool>.CreateSucceeded(true, "Successfully updated chat history");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured when updating all entities");
            }
        }
    }
}
