using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class ResetPasswordArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    public string Token {get; set;}
    [Required]
    public string Password {get; set;}
}