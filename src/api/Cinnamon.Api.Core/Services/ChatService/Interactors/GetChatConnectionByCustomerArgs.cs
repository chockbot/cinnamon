using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class GetChatConnectionByCustomerArgs : IInteractor
    {
        public int CustomerId { get; set; }
    }
}
