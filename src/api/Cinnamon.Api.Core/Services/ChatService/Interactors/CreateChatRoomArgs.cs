using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class CreateChatRoomArgs : IInteractor
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
    }
}
