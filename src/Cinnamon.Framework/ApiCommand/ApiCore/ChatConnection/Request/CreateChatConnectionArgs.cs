using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Request
{
    public class CreateChatConnectionArgs
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
    }
}
