using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatConnection;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using static Cinnamon.Framework.Enums.Enums;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ChatConnection
{
    public class ChatConnectionRepository : IChatConnectionRepository
    {
        private readonly IDataStore dataStore;

        public ChatConnectionRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<AppResult<bool>> Create(int customerId, string connectionId, string userAgent, bool isConnected)
        {
            try
            {
                var result = await dataStore.ChatConnection.Add(new Data.Repository.Entities.ChatConnection
                {
                    CustomerId = customerId,
                    ConnectionId = connectionId,
                    UserAgent = userAgent,
                    IsConnected = isConnected
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                return AppResult<bool>.CreateSucceeded(result.Succeeded, "Successully created chat connection");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured when creating chat connection");
            }
        }

        public async Task<AppResult<IEnumerable<ChatConnectionDTO>>> GetChatConnectionByCustomer(int customerId)
        {
            try
            {
                var result = await dataStore.ChatConnection.FindAsync(c => c.CustomerId == customerId && c.IsConnected, int.MaxValue);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ChatConnectionDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var chatConnections = result.Result.Select(c =>
                {
                    var chatHistoryDto = new ChatConnectionDTO
                    {
                       ConnectionId = c.ConnectionId,
                       CustomerId = c.CustomerId,
                       IsConnected = c.IsConnected,
                       UserAgent = c.UserAgent
                    };

                    return chatHistoryDto;
                });

                return AppResult<IEnumerable<ChatConnectionDTO>>.CreateSucceeded(chatConnections, "Successfully retrieved chat connections");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatConnectionDTO>>.CreateFailed(ex, "An error occured when retrieving chat connections");
            }
        }

        public async Task<AppResult<bool>> Update(int customerId, string connectionId, bool isConnected)
        {
            try
            {
                var result = await dataStore.ChatConnection.FindFirstAsync(c => c.CustomerId == customerId && c.ConnectionId == connectionId);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
                }

                result.Result.IsConnected = isConnected;

                var updatedRes = await dataStore.ChatConnection.Update(result.Result);

                if (!updatedRes.Succeeded || updatedRes.Result == null)
                {
                    return AppResult<bool>.CreateFailed(
                        new ApplicationException("An error occured when updating all entities"), "An error occured when updating all entities");
                }

                return AppResult<bool>.CreateSucceeded(true, "Successfully updated chat connection");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured when updating all entities");
            }
        }
    }
}
