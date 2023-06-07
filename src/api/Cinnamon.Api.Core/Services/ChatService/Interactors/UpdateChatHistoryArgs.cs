using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class UpdateChatHistoryArgs : IInteractor
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public bool IsViewed { get; set; }
    }
}
