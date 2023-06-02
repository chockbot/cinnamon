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
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly IDataStore dataStore;

        public ChatRoomRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<AppResult<ChatRoomDTO>> Create(int fromUserId, int toUserId)
        {
            try
            {
                var entity = new Entities.ChatRoom
                {
                    FromUserId         = fromUserId,
                    ToUserId           = toUserId
                };

                var result = await dataStore.ChatRooms.Add(entity);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                var created = result.Result;

                return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                {
                   ChatRoomId       = created.Id,
                   FromUserId       = created.FromUserId,
                   ToUserId         = created.ToUserId,
                }, "Successully created chat room");
            }
            catch (Exception ex)
            {
                return AppResult<ChatRoomDTO>.CreateFailed(ex, "An error occured when creating chat room");
            }
        }

        public async Task<AppResult<IEnumerable<ChatRoomDTO>>> GetChatRoomsByUserId(int userId)
        {
            try
            {
                Expression<Func<Entities.ChatRoom, bool>> filter =
                a => (a.FromUserId == userId || a.ToUserId == userId);

                var includes = new List<Expression<Func<Entities.ChatRoom, object>>>
                {
                    a => a.ChatHistory,
                    //a => a.FromCustomer,
                    //a => a.ToCustomer
                };

                var result = await dataStore.ChatRooms.GetChatRoomsByUserId(filter, includes);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var chatRooms = result.Result.GroupBy(c => new
                {
                    FromFirstName   = c.FromCustomer.FirstName,
                    //FromLastName    = c.Customer.LastName,
                    //FromUserId      = c.FromUserId,
                    //ToUserId        = c.ToUserId,
                    //ToFirstName     = c.ToCustomer.FirstName,
                    //ToLastName      = c.ToCustomer.LastName,
                    //ChatRoomId      = c.Id,
                    //Message         = c.ChatHistory?.FirstOrDefault().Message,
                    //FromProfilePath = c.Customer.ProfilePath,
                    //ToProfilePath   = c.ToCustomer?.ProfilePath
                }).Select(c         => new ChatRoomDTO
                {     
                    FromFirstName   = c.Key.FromFirstName,
                    //FromLastName    = c.Key.FromLastName,
                    //FromUserId      = c.Key.FromUserId,
                    //ToUserId        = c.Key.ToUserId,
                    //ToFirstName     = c.Key.ToFirstName,
                    //ToLastName      = c.Key.ToLastName,
                    //ChatRoomId      = c.Key.ChatRoomId,
                    //Message         = c.Key.Message,
                    //FromProfilePath = c.Key.FromProfilePath,
                    //ToProfilePath   = c.Key.ToProfilePath
                });

                return AppResult<IEnumerable<ChatRoomDTO>>.CreateSucceeded(chatRooms, "Successfully retrieved chat room");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatRoomDTO>>.CreateFailed(ex, "An error occured when retrieving chat room");
            }
        }
    }
}
