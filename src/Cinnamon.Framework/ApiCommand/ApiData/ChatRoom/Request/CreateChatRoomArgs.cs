using Cinnamon.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request
{
    public class CreateChatRoomArgs
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public Enums.Enums.ChatType ChatType { get; set; }
        public string GroupName { get; set; }
        public string ChatName { get; set; }
    }
}
