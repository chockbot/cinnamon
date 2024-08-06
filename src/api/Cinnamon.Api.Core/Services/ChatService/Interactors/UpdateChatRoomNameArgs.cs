using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors;

public class UpdateChatRoomNameArgs : IInteractor
{
    public int ChatRoomId { get; set; }
    public string ChatRoomName { get; set; }
}