using Cinnamon.Framework.Interactor;
using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class CreateChatConnectionArgs : IInteractor
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
    }
}
