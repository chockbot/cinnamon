using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors
{
    public class GetChatHistoryByChatRoomIdArgs : IInteractor
    {
        public int? ChatRoomId { get; set; }
        public int? PageIndex { get; set; }
        public int? CountPerPage { get; set; }
    }
}
