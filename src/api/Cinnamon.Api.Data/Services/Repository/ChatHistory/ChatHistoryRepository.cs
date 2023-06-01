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
                var entity = new Entities.ChatHistory
                {
                    ChatRoomId         = chatRoomId,
                    FromUserId         = fromUserId,
                    ToUserId           = toUserId,
                    Message            = message,
                    IsViewed           = isViewed,
                    FromConnectionId   = fromConnectionId,
                    ToConnectionId     = toConnectionId
                };

                var result = await dataStore.ChatHistory.Add(entity);
                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<ChatHistoryDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }
                var created = result.Result;

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

                var chatHistories = result.Result.Select(c => new Entities.ChatHistory
                {
                    ChatRoomId = c.ChatRoomId,
                    FromUserId = c.FromUserId,
                    ToUserId   = c.ToUserId,
                    IsViewed   = isViewed.GetValueOrDefault()
                });

                var updatedRes = await dataStore.ChatHistory.UpdateRange(chatHistories);

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
