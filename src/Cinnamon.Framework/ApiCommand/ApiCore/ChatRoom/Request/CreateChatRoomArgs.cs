using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinnamon.Framework.Enums;
namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request
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
