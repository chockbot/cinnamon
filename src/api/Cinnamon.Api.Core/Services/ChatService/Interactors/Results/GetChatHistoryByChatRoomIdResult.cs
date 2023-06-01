using Cinnamon.Framework.ApiCommand.ApiCore;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors.Results
{
    public class GetChatHistoryByChatRoomIdResult
    {
        public ErrorInfo? ErrorInfo { get; set; }
        public Pagination? Pagination { get; set; }
        public IEnumerable<ChatHistory> ChatHistories { get; set; }
    }

    public class ChatHistory
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsViewed { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
