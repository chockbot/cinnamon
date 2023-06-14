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
using System.Linq;
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
                bool isExist = false;
                var chatRoomId = 0;

                var chatMemberFromResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == fromUserId);
                var chatMemberToResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == toUserId);

                if (chatMemberFromResult != null || chatMemberToResult != null)
                {
                    foreach (var item in chatMemberToResult.Result)
                    {
                        if (chatMemberFromResult.Result.Select(c => c.ChatRoomId).Contains(item.ChatRoomId))
                        {
                            isExist = true;
                            chatRoomId = item.ChatRoomId;
                            break;
                        }
                    }
                }

                if (!isExist)
                {
                    var entity = new Entities.ChatRoom
                    {
                        Name = string.Empty,
                        LatestMessage = string.Empty,
                        CreatedOn = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now)
                    };

                    var result = await dataStore.ChatRooms.Add(entity);

                    if (!result.Succeeded || result.Result == null)
                    {
                        return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                    }

                    var newChatRoom = result.Result;


                    var chatMemberEntity = new List<Entities.ChatMember>
                    {
                        new ChatMember
                        {
                            ChatRoomId = newChatRoom.Id,
                            CustomerId = fromUserId
                        },
                        new ChatMember
                        {
                            ChatRoomId = newChatRoom.Id,
                            CustomerId = toUserId
                        }
                    };

                    var chatMemberResult = await dataStore.ChatMember.AddRange(chatMemberEntity);

                    if (!chatMemberResult.Succeeded || chatMemberResult.Result == null)
                    {
                        return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                    }

                    return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                    {
                        ChatRoomId = newChatRoom.Id
                    }, "Successully created chat room");
                }
                else
                {
                    return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                    {
                        ChatRoomId = chatRoomId
                    }, "Successully created chat room");
                }
            }
            catch (Exception ex)
            {
                return AppResult<ChatRoomDTO>.CreateFailed(ex, "An error occured when creating chat room");
            }
        }

       
    }
}
