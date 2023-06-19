namespace Cinnamon.Web.Models.Entities
{
    public class ChatMember
    {
        public int FromUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string? FromProfilePath { get; set; }
    }
}
