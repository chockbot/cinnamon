using Cinnamon.Framework.Enums;
using System.ComponentModel.DataAnnotations;
using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;

public class CreateChatHistoryArgs
{
    public int ChatRoomId { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public string Message { get; set; }
    public bool IsViewed { get; set; }
    public string FromConnectionId { get; set; }
    public string ToConnectionId { get; set; }
    public ChatHistoryType ChatHistoryType { get; set; }
}