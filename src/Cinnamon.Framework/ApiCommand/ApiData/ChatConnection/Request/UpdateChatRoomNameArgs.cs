using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;

public class UpdateChatRoomNameArgs
{
    [Required]
    public int ChatRoomId { get; set; }


    [Required]
    public string NewChatRoomName { get; set; }
}
