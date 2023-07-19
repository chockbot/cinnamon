using Cinnamon.Framework.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;

public class UpdateChatHistoryArgs
{
    public int ChatRoomId { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public bool IsViewed { get; set; }
}