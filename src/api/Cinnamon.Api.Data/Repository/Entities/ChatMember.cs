using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatMember: BaseEntity
    {
        public int ChatRoomId { get; set; }
        public int CustomerId { get; set; }
        public bool HasLeft { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
        public int ChatMemberType { get; set; }

    }
}
