using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICreateChatHistoryHandler createChatHistoryHandler;
        private readonly IUpdateConnectionIdHandler updateConnectionIdHandler;
        public ChatHub(ICreateChatHistoryHandler createChatHistoryHandler, IUpdateConnectionIdHandler updateConnectionIdHandler)
        {
            this.createChatHistoryHandler = createChatHistoryHandler;
            this.updateConnectionIdHandler = updateConnectionIdHandler;
        }
        public async Task SendMessage(int chatRoomId, int fromUserId, int toUserId, string message, string fromConnectionId, string toConnectionId)
        {
            await Clients.Client(toConnectionId).SendAsync("ReceiveMessage", chatRoomId, fromUserId, toUserId, message);

            await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
            {
                ChatRoomId       = chatRoomId,
                FromConnectionId = fromConnectionId,
                FromUserId       = fromUserId,
                ToConnectionId   = toConnectionId,
                ToUserId         = toUserId,
                IsViewed         = false,
                Message          = message
            });
        }

        public async override Task OnConnectedAsync()
        {
            await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = Context.ConnectionId
            });
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await updateConnectionIdHandler.ExecuteAsync(new Services.AccountService.Interactors.UpdateConnectionIdArgs
            {
                ConnectionId = string.Empty
            });
        }
    }
}
