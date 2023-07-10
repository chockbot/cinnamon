using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors.Results
{
    public class GetChatConnectionByCustomerResult
    {
        public IEnumerable<ChatConnection> ChatConnections { get; set; }
    }

    public class ChatConnection
    {
        public int CustomerId { get; set; }
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
    }
}
