using Cinnamon.Framework.ApiCommand.ApiCore;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors.Results
{
    public class CreateChatRoomResult
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string GroupName { get; set; }
    }
}
