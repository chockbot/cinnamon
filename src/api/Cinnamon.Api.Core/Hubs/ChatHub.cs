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
        public ChatHub(ICreateChatHistoryHandler createChatHistoryHandler, IUpdateConnectionIdHandler updateConnectionIdHandler, IGetCustomerByIdHandler getCustomerByIdHandler)
        {
            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
            this.getCustomerByIdHandler = getCustomerByIdHandler;
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
                    await Clients.Client(toUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.Now}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}");
                }

                if (!string.IsNullOrEmpty(fromUser.ConnectionId))
                {
                    await Clients.Client(fromUser.ConnectionId).SendAsync("ReceiveMessage", $"{chatRoomId}|{DateTime.Now}|{fromUser.ConnectionId}|{fromUserId}|{fromFirstName}|{fromLastName}|{fromProfilePath}|{false}|{messageInput}|{toUser.ConnectionId}|{toUser.Id}|{toUser.FirstName}|{toUser.LastName}|{toUser.ProfileImg}");
                }

                await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                {
                    ChatRoomId       = chatRoomId,
                    FromConnectionId = fromUser.ConnectionId ?? string.Empty,
                    FromUserId       = fromUserId,
                    ToConnectionId   = toUser.ConnectionId ?? string.Empty,
                    ToUserId         = toUserId,
                    IsViewed         = !string.IsNullOrEmpty(toUser.ConnectionId),
                    Message          = messageInput
                });
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
