namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatHistory: BaseEntity
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsViewed { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public int ChatHistoryType { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
        public virtual Customer FromCustomer { get; set; }
        public virtual Customer ToCustomer { get; set; }
    }
}
