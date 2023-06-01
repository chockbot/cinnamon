namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatRoom: BaseEntity
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
    }
}
