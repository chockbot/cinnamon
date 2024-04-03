using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;

public class RequestMessageArgs
{
    [Required]
    public int ProviderId {get; set;}
}