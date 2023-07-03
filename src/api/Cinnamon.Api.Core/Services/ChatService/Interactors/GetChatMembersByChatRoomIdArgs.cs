using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class GetChatMembersByChatRoomIdArgs : IInteractor
    {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
    }
}
