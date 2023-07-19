using Cinnamon.Framework.Interactor;
using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class UpdateChatConnectionArgs : IInteractor
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public bool IsConnected { get; set; }
    }
}
