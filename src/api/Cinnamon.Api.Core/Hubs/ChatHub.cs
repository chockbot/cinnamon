using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.Authorization;
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
        public ChatHub(ICreateChatHistoryHandler createChatHistoryHandler, IUpdateConnectionIdHandler updateConnectionIdHandler, IGetCustomerByIdHandler getCustomerByIdHandler, IGetChatRoomsByUserIdHandler getChatRoomsByUserIdHandler)
        {
            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
            this.getCustomerByIdHandler = getCustomerByIdHandler;
            this.getChatRoomsByUserIdHandler = getChatRoomsByUserIdHandler;
        }
        public async Task SendMessage(int chatRoomId, string fromConnectionId, int fromUserId, string fromLastName, string fromFirstName, string fromProfilePath, int toUserId, string messageInput)
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
                    await Clients.Client(toUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}");
                }

                if (!string.IsNullOrEmpty(fromUser.ConnectionId))
                {
                    await Clients.Client(fromUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.UtcNow}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}");
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
