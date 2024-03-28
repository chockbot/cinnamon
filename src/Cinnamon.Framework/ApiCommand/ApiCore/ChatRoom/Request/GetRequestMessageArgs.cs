using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;

public class GetRequestMessageArgs
{
    [Required]
    public string Token {get; set;}
    
    [Required]
    public string Guid {get; set;}
}