using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Web.Models.Entities
{
    public class ChatHistory
    {
        public int ChatHistoryId { get; set; }
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string? FromProfilePath { get; set; }
        public int ToUserId { get; set; }
        public string ToLastName { get; set; }
        public string ToFirstName { get; set; }
        public string? ToProfilePath { get; set; }
        public string Message { get; set; }
        public bool IsViewed { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public DateTime DateCreated { get; set; }
        public ChatHistoryType ChatHistoryType { get; set; }
    }
}
