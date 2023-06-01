using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Barangay;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatHistory;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.City;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Customer;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Region;
using Cinnamon.Framework.Common;
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

        public async Task<AppResult<ChatHistoryDTO>> Create(int chatRoomActivityId, int fromUserId, int toUserId, string message, bool isViewed, string fromConnectionId, string toConnectionId)
        {
            try
            {
                var entity = new Entities.ChatHistory
                {
                    ChatRoomActivityId = chatRoomActivityId,
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
                   ChatRoomActivityId = created.ChatRoomActivityId,
                   FromUserId         = created.FromUserId,
                   ToUserId           = created.ToUserId,
                   Message            = created.Message,
                   IsViewed           = created.IsViewed,
                   FromConnectionId   = created.FromConnectionId,
                   ToConnectionId     = created.ToConnectionId
                }, "Successully created chat history");
            }
            catch (Exception ex)
            {
                return AppResult<ChatHistoryDTO>.CreateFailed(ex, "An error occured when creating chat history");
            }
        }
    }
}
