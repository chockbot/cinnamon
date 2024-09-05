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
using System;
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

        public async Task<AppResult<ChatRoomDTO>> Create(int fromUserId, int toUserId, Enums.ChatType chatType, string groupName, string chatName)
        {
            try
            {
                bool isExist = false;
                var chatRoomId = 0;

                if (chatType == Enums.ChatType.PrivateMessage)
                {
                    var chatMemberFromResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == fromUserId);
                    var chatMemberToResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == toUserId);

                    if (chatMemberFromResult != null || chatMemberToResult != null)
                    {
                        var chatRoomsFromResult = await dataStore.ChatRooms.FindAsync(c => chatMemberFromResult.Result.Select(cm => cm.ChatRoomId).Contains(c.Id) && c.ChatType == (int)Enums.ChatType.PrivateMessage);
                        var chatRoomsToResult = await dataStore.ChatRooms.FindAsync(c => chatMemberToResult.Result.Select(cm => cm.ChatRoomId).Contains(c.Id) && c.ChatType == (int)Enums.ChatType.PrivateMessage);

                        if (chatRoomsFromResult != null || chatRoomsToResult != null)
                        {
                            foreach (var item in chatRoomsToResult.Result)
                            {
                                if (chatRoomsFromResult.Result.Select(c => c.Id).Contains(item.Id))
                                {
                                    isExist = true;
                                    chatRoomId = item.Id;
                                    break;
                                }
                            }
                        }
                    }

                    if (!isExist)
                    {
                        return await CreateChatRoom(fromUserId, toUserId, chatType, groupName, chatName);
                    }
                    else
                    {
                        return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                        {
                            ChatRoomId = chatRoomId
                        }, "Successully created chat room");
                    }
                }
                else
                {
                    var chatMemberToResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == toUserId && c.ChatMemberType == (int)Enums.ChatMemberType.ActivityProvider);

                    if (chatMemberToResult.Result.Count() > 0)
                    {
                        var existingChatrooms = chatMemberToResult.Result.Select(c => c.ChatRoomId);

                        var chatroom = await dataStore.ChatRooms.FindFirstAsync(c => existingChatrooms.Contains(c.Id) && c.ChatType == (int)Enums.ChatType.GroupChat);
                        if (chatroom == null || chatroom.Result == null)
                        {
                            return await CreateChatRoom(fromUserId, toUserId, chatType, groupName, chatName);
                        }
                        else
                        {
                            var chatMemberFromResult = await dataStore.ChatMember.FindAsync(c => c.CustomerId == fromUserId);

                            if (chatMemberFromResult != null) 
                            {
                                if (!chatMemberFromResult.Result.Any(c => c.ChatRoomId == chatroom.Result.Id && !c.HasLeft))
                                {
                                    var chatMemberEntity = new List<Entities.ChatMember>
                                    {
                                        new ChatMember
                                        {
                                            ChatRoomId = chatroom.Result.Id,
                                            CustomerId = fromUserId,
                                            ChatMemberType = (int)Enums.ChatMemberType.Customer
                                        },
                                    };

                                    var chatMemberResult = await dataStore.ChatMember.AddRange(chatMemberEntity);

                                    if (!chatMemberResult.Succeeded || chatMemberResult.Result == null)
                                    {
                                        return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(chatMemberResult.Message), chatMemberResult.Message);
                                    }

                                    return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                                    {
                                        ChatRoomId = chatroom.Result.Id,
                                        GroupName = chatroom.Result.GroupName
                                    }, "Successully created chat room");
                                }
                                else
                                {
                                    return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                                    {
                                        ChatRoomId = chatroom.Result.Id,
                                        GroupName = chatroom.Result.GroupName
                                    }, "Successully created chat room");
                                }
                            }
                            else
                            {
                                return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                                {
                                    ChatRoomId = chatroom.Result.Id,
                                    GroupName = chatroom.Result.GroupName
                                }, "Successully created chat room");
                            }
                        }
                    }
                    else
                    {
                        return await CreateChatRoom(fromUserId, toUserId, chatType, groupName, chatName);
                    }
                }
            }
            catch (Exception ex)
            {
                return AppResult<ChatRoomDTO>.CreateFailed(ex, "An error occured when creating chat room");
            }
        }

        private async Task<AppResult<ChatRoomDTO>> CreateChatRoom(int fromUserId, int toUserId, Enums.ChatType chatType, string groupName, string chatName)
        {
            var entity = new Entities.ChatRoom
            {
                Name = chatName,
                LatestMessage = string.Empty,
                CreatedOn = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                ChatType = (int)chatType,
                GroupName = groupName
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
                                            CustomerId = fromUserId,
                                            ChatMemberType = (int)Enums.ChatMemberType.Customer
                                        },
                                        new ChatMember
                                        {
                                            ChatRoomId = newChatRoom.Id,
                                            CustomerId = toUserId,
                                            ChatMemberType = (int)Enums.ChatMemberType.ActivityProvider
                                        }
                                    };

            var chatMemberResult = await dataStore.ChatMember.AddRange(chatMemberEntity);

            if (!chatMemberResult.Succeeded || chatMemberResult.Result == null)
            {
                return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
            {
                ChatRoomId = newChatRoom.Id,
                GroupName = newChatRoom.GroupName
            }, "Successully created chat room");
        }

        public async Task<AppResult<ChatRoomDTO>> UpdateChatRoomName(int chatRoomId, string newChatRoomName)
        {
            try
            {
                var chatRoom = await dataStore.ChatRooms.GetByIdAsync(chatRoomId);

                if (chatRoom == null || chatRoom.Result == null)
                {
                    return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException("Chat room not found"), "Chat room not found");
                }

                chatRoom.Result.Name = newChatRoomName;
                var result = await dataStore.ChatRooms.Update(chatRoom.Result);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<ChatRoomDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                return AppResult<ChatRoomDTO>.CreateSucceeded(new ChatRoomDTO
                {
                    ChatRoomId = chatRoom.Result.Id,
                    GroupName = chatRoom.Result.GroupName
                }, "Successfully updated chat room name");
            }
            catch (Exception ex)
            {
                return AppResult<ChatRoomDTO>.CreateFailed(ex, "An error occurred when updating chat room name");
            }
        }

    }
}
