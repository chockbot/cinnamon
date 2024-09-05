using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;

public class GetChatMemberArgs
{
    [Required]
    public int ChatRoomId { get; set; }


    [Required]
    public int UserId { get; set; }
}
