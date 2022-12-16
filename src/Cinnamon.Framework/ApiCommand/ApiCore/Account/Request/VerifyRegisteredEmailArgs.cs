using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class VerifyRegisteredEmailArgs
{
    [Required]
    public string UserId {get; set;}
    [Required]
    public string Token {get; set;}
}