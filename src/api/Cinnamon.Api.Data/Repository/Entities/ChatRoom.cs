namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatRoom: BaseEntity
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual IList<ChatHistory> ChatHistory { get; set; }
    }
}
