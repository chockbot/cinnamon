using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class GetChatRoomsByUserIdArgs : IInteractor
    {
        public int UserId { get; set; }
    }
}
