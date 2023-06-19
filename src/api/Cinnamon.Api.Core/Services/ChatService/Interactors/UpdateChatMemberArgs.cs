using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class UpdateChatMemberArgs : IInteractor
    {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public bool HasLeft { get; set; }
    }
}
