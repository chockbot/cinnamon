using Cinnamon.Framework.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Request;

public class CreateChatHistoryArgs
{
    public int ChatRoomActivityId { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public string Message { get; set; }
    public bool IsViewed { get; set; }
    public string FromConnectionId { get; set; }
    public string ToConnectionId { get; set; }
}