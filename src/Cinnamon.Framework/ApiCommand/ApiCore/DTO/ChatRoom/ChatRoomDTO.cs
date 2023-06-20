using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.ChatRoom
{
    public class ChatRoomDTO
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string ToLastName { get; set; }
        public string ToFirstName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Message { get; set; }
        public string? FromProfilePath { get; set; }
        public string? ToProfilePath { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public bool HasNewMessage { get; set; }
        public string? FromProfileLink { get; set; }
        public Enums.Enums.ChatType ChatType { get; set; }
        public Enums.Enums.ChatMemberType ChatMemberType { get; set; }
        public string GroupName { get; set; }
        public string ChatName { get; set; }
    }
}
