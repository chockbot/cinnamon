using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatRoom: BaseEntity
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public virtual Customer FromCustomer { get; set; }
        public virtual Customer ToCustomer { get; set; }
        public virtual IList<ChatHistory> ChatHistory { get; set; }
    }
}
