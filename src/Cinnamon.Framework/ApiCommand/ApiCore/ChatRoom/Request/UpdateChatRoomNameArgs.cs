using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;

public class UpdateChatRoomNameArgs
{
    [Required]
    public int ChatRoomId { get; set; }

    [Required]
    public string ChatRoomName { get; set; }
}