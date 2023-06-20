using Cinnamon.Framework.Enums;

namespace Cinnamon.Web.Models.Entities
{
    public class ChatMember
    {
        public int FromUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string? FromProfilePath { get; set; }
        public string? FromProfileLink { get; set; }
        public Framework.Enums.Enums.ChatMemberType ChatMemberType { get; set; }
    }
}
