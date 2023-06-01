using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.ChatHistory
{
    public class ChatHistoryDTO
    {
        public int ChatRoomActivityId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public bool IsViewed { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
