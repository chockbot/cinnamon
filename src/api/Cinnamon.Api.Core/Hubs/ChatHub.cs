using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ICreateChatHistoryHandler createChatHistoryHandler;
        private readonly IUpdateConnectionIdHandler updateConnectionIdHandler;
        private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
        private readonly IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler;
        private readonly IUpdateChatMemberHandler updateChatMemberHandler;
        public ChatHub(ICreateChatHistoryHandler createChatHistoryHandler, IUpdateConnectionIdHandler updateConnectionIdHandler, IGetCustomerByIdHandler getCustomerByIdHandler, IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler, IUpdateChatMemberHandler updateChatMemberHandler)
        {
            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
            this.getCustomerByIdHandler = getCustomerByIdHandler;
            this.getChatRoomsByUserIdHandler = getChatRoomsByUserIdHandler;
            this.updateChatMemberHandler = updateChatMemberHandler;
        }
        public async Task SendMessage(string payload)
        {
            var splitted = payload.Split("|");

            int chatRoomId          = int.Parse(splitted[0]);
            string fromConnectionId = splitted[1];
            int fromUserId          = int.Parse(splitted[2]);
            string fromLastName     = splitted[3];
            string fromFirstName    = splitted[4];
            string fromProfilePath  = splitted[5];
            int toUserId            = int.Parse(splitted[6]);
            string messageInput     = splitted[7];
            string groupName        = splitted[8];
            Enums.ChatType chatType = (Enums.ChatType)int.Parse(splitted[9]);

            if (chatType == Enums.ChatType.PrivateMessage)
            {
                var toCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
                {
                    Id = toUserId
                });

                var fromCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
                {
                    Id = fromUserId
                });

                if (toCustomerResult.Succeeded && toCustomerResult.Result != null && fromCustomerResult.Succeeded && fromCustomerResult.Result != null)
                {
                    var fromUser = fromCustomerResult.Result;
                    var toUser = toCustomerResult.Result;

                    if (!string.IsNullOrEmpty(toUser.ConnectionId))
                    {
                        await Clients.Client(toUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}|{(int)chatType}");
                    }

                    if (!string.IsNullOrEmpty(fromUser.ConnectionId))
                    {
                        await Clients.Client(fromUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}|{(int)chatType}");
                    }

                    await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                    {
                        ChatRoomId = chatRoomId,
                        FromConnectionId = fromUser.ConnectionId ?? string.Empty,
                        FromUserId = fromUserId,
                        ToConnectionId = toUser.ConnectionId ?? string.Empty,
                        ToUserId = toUserId,
                        IsViewed = false,
                        Message = messageInput
                    });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(groupName))
                {
                    await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{groupName}|{(int)chatType}");
                    
                    await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                    {
                        ChatRoomId = chatRoomId,
                        FromConnectionId =  string.Empty,
                        FromUserId = fromUserId,
                        ToConnectionId = string.Empty,
                        ToUserId = toUserId,
                        IsViewed = false,
                        Message = messageInput
                    });
                }
            }
        }

        public async Task AddToGroup(string groupName, int userId)
        {
            var toCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
            {
                Id = userId
            });

            if (!string.IsNullOrEmpty(groupName))
            {
                if (toCustomerResult.Succeeded && toCustomerResult.Result != null)
                {
                    await Groups.AddToGroupAsync(toCustomerResult.Result.ConnectionId, groupName);
                }
            }
        }

        public async Task RemoveFromGroup(string groupName, int userId, int chatRoomId)
        {
            var toCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
            {
                Id = userId
            });

            if (!string.IsNullOrEmpty(groupName))
            {
                if (toCustomerResult.Succeeded && toCustomerResult.Result != null)
                {
                    await Groups.RemoveFromGroupAsync(toCustomerResult.Result.ConnectionId, groupName);
                }
            }

            var updateChatMemberResult = await updateChatMemberHandler.ExecuteAsync(new Services.ChatService.Interactors.UpdateChatMemberArgs
            {
                ChatRoomId = chatRoomId,
                HasLeft = true,
                UserId = userId
            });
        }

        public async Task SendChatStatus(int customerId)
        {
            var result = await getChatRoomsByUserIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatRoomsByUserIdArgs
            {
                UserId = customerId
            });

            if (result.Succeeded && result != null) 
            {
                foreach (var item in result.Result.ChatRooms)
                {
                    if(!string.IsNullOrEmpty(item.FromConnectionId))
                        await Clients.Client(item.FromConnectionId).SendAsync("UpdateChatStatus", $"{item.ChatRoomId}|{string.Empty}");
                }
            }
        }

        public async Task SendTypingStatusKeyDown(int chatRoomId, int fromUserId, string fromLastName, string fromFirstName)
        {
            var result = await getChatRoomsByUserIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatRoomsByUserIdArgs
            {
                UserId = fromUserId
            });

            if (result.Succeeded && result != null)
            {

                foreach (var item in result.Result.ChatRooms.Where(c => c.ChatRoomId == chatRoomId))
                {
                    if (!string.IsNullOrEmpty(item.FromConnectionId))
                        await Clients.Client(item.FromConnectionId).SendAsync("UpdateTypingStatusKeyDown", $"{chatRoomId}|{fromLastName}|{fromFirstName}");
                }
            }
        }

        public async Task SendTypingStatusKeyUp(int chatRoomId, int fromUserId)
        {
            var result = await getChatRoomsByUserIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatRoomsByUserIdArgs
            {
                UserId = fromUserId
            });

            if (result.Succeeded && result != null)
            {

                foreach (var item in result.Result.ChatRooms.Where(c => c.ChatRoomId == chatRoomId))
                {
                    if (!string.IsNullOrEmpty(item.FromConnectionId))
                        await Clients.Client(item.FromConnectionId).SendAsync("UpdateTypingStatusKeyUp", $"{chatRoomId}");
                }
            }
        }

        public async override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue("UserId");

            await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = Context.ConnectionId,
                CustomerId = Convert.ToInt32(userId)
            });

            var result = await getChatRoomsByUserIdHandler.ExecuteAsync(new Services.ChatService.Interactors.GetChatRoomsByUserIdArgs
            {
                UserId = Convert.ToInt32(userId)
            });

            if (result.Succeeded && result != null)
            {
                foreach (var item in result.Result.ChatRooms)
                {
                    if (!string.IsNullOrEmpty(item.FromConnectionId))
                        await Clients.Client(item.FromConnectionId).SendAsync("UpdateChatStatus", $"{item.ChatRoomId}|{Context.ConnectionId}");
                }
            }
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirstValue("UserId");

            await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = string.Empty,
                CustomerId = Convert.ToInt32(userId)
            });
        }
    }
}
