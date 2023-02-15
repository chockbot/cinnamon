using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Request;

public class CreateResetPasswordArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    public string Guid {get; set;}
    [Required]
    public string Token {get; set;}
    public bool IsUsed {get; set;} = false;
}