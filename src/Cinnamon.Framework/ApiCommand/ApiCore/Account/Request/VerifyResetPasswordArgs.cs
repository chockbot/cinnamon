using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class VerifyResetPasswordArgs
{
    [Required]
    public string Token {get; set;}
    [Required]
    public string Guid {get; set;}
    [Required]
    public string NewPassword {get; set;}
}