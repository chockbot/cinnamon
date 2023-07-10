using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request
{
    public class UpdateChatConnectionArgs
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public bool IsConnected { get; set; }
    }
}
