using Cinnamon.Framework.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Request;

public class GetChatHistoryByChatRoomIdArgs
{
    public int? ChatRoomId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? UserId { get; set; }
}