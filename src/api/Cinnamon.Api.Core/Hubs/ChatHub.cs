using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using static Cinnamon.Framework.Enums.Enums;

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
        private readonly ILogger _logger;

        public ChatHub(ICreateChatHistoryHandler createChatHistoryHandler, IUpdateConnectionIdHandler updateConnectionIdHandler, IGetCustomerByIdHandler getCustomerByIdHandler, IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler, IUpdateChatMemberHandler updateChatMemberHandler, ILogger<ChatHub> logger)
        {
            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
            this.getCustomerByIdHandler = getCustomerByIdHandler;
            this.getChatRoomsByUserIdHandler = getChatRoomsByUserIdHandler;
            this.updateChatMemberHandler = updateChatMemberHandler;
            this._logger = logger;
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
            Enums.ChatHistoryType chatHistoryType = (Enums.ChatHistoryType)int.Parse(splitted[10]);
            string chatName         = splitted[11];

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
                        await Clients.Client(toUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}|{(int)chatType}|{(int)chatHistoryType}");
                    }

                    if (!string.IsNullOrEmpty(fromUser.ConnectionId))
                    {
                        await Clients.Client(fromUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}|{(int)chatType}|{(int)chatHistoryType}");
                    }

                    await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                    {
                        ChatRoomId = chatRoomId,
                        FromConnectionId = fromUser.ConnectionId ?? string.Empty,
                        FromUserId = fromUserId,
                        ToConnectionId = toUser.ConnectionId ?? string.Empty,
                        ToUserId = toUserId,
                        IsViewed = false,
                        Message = messageInput,
                        ChatHistoryType = Enums.ChatHistoryType.Message
                    });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(groupName))
                {
                    await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{groupName}|{(int)chatType}|{(int)chatHistoryType}|{false}|{chatName}");
                    
                    await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                    {
                        ChatRoomId = chatRoomId,
                        FromConnectionId =  string.Empty,
                        FromUserId = fromUserId,
                        ToConnectionId = string.Empty,
                        ToUserId = toUserId,
                        IsViewed = false,
                        Message = messageInput,
                        ChatHistoryType = Enums.ChatHistoryType.Message
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

        public async Task AddToGroupAfterPayment(string payload)
        {
            var splitted           = payload.Split("|");
            string groupName       = splitted[0];
            int chatRoomId         = int.Parse(splitted[1]);
            int fromUserId         = int.Parse(splitted[2]);
            string fromFirstName   = splitted[3];
            string fromLastName    = splitted[4];
            string fromProfilePath = splitted[5];
            string chatName        = splitted[6];

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{fromFirstName} {fromLastName} has joined the group.|{groupName}|{(int)ChatType.GroupChat}|{(int)ChatHistoryType.Notification}|{false}|{chatName}");

            await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
            {
                ChatRoomId = chatRoomId,
                FromConnectionId = string.Empty,
                FromUserId = fromUserId,
                ToConnectionId = string.Empty,
                ToUserId = 0,
                IsViewed = false,
                Message = $"{fromFirstName} {fromLastName} has joined the group.",
                ChatHistoryType = Enums.ChatHistoryType.Notification
            });

            var toCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
            {
                Id = fromUserId
            });

            if (!string.IsNullOrEmpty(groupName))
            {
                if (toCustomerResult.Succeeded && toCustomerResult.Result != null)
                {
                    await Groups.AddToGroupAsync(toCustomerResult.Result.ConnectionId, groupName);
                }
            }
        }

        public async Task RemoveFromGroup(string payload)
        {
            var splitted           = payload.Split("|");
            string groupName       = splitted[0];
            int chatRoomId         = int.Parse(splitted[1]);
            int fromUserId         = int.Parse(splitted[2]);
            string fromFirstName   = splitted[3];
            string fromLastName    = splitted[4];
            string fromProfilePath = splitted[5];

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{fromFirstName} {fromLastName} has left the group.|{groupName}|{(int)ChatType.GroupChat}|{(int)ChatHistoryType.Notification}|{true}|{string.Empty}");

            await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
            {
                ChatRoomId = chatRoomId,
                FromConnectionId = string.Empty,
                FromUserId = fromUserId,
                ToConnectionId = string.Empty,
                ToUserId = 0,
                IsViewed = false,
                Message = $"{fromFirstName} {fromLastName} has left the group.",
                ChatHistoryType = Enums.ChatHistoryType.Notification
            });

            var toCustomerResult = await getCustomerByIdHandler.ExecuteAsync(new Services.AccountService.Interactors.GetCustomerByIdArgs
            {
                Id = fromUserId
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
                UserId = fromUserId
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

            _logger.LogInformation($"UserId {userId} is connected on connection id {Context.ConnectionId}");

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
                    if (item.ChatType == ChatType.PrivateMessage)
                    {
                        if (!string.IsNullOrEmpty(item.FromConnectionId))
                            await Clients.Client(item.FromConnectionId).SendAsync("UpdateChatStatus", $"{item.ChatRoomId}|{Context.ConnectionId}");
                    }
                    else
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, item.GroupName);
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirstValue("UserId");

            _logger.LogInformation($"UserId {userId} is disconnected on connection id {Context.ConnectionId}. exception: {exception?.Message}");

            await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = string.Empty,
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
                    if (item.ChatType == ChatType.PrivateMessage)
                    {
                        if (!string.IsNullOrEmpty(item.FromConnectionId))
                            await Clients.Client(item.FromConnectionId).SendAsync("UpdateChatStatus", $"{item.ChatRoomId}|{string.Empty}");
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
