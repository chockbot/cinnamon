using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request
{
    public class GetChatMembersByChatRoomIdArgs
    {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public bool HasLeft { get; set; }
    }
}
