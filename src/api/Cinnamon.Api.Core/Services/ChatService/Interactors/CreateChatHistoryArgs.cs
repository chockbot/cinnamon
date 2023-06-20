using Cinnamon.Framework.Interactor;
using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class CreateChatHistoryArgs : IInteractor
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsViewed { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public ChatHistoryType ChatHistoryType { get; set; }
    }
}
