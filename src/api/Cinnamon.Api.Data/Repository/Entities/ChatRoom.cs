using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatRoom: BaseEntity
    {
        public string Name { get; set; }
        public string LatestMessage { get; set; }
        public virtual IList<ChatMember> ChatMembers { get; set; }
    }
}
