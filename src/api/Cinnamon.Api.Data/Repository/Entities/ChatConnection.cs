namespace Cinnamon.Api.Data.Repository.Entities
{
    public class ChatConnection : BaseEntity
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
