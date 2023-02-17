using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class ResetPasswordArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    public string ValidationRoute {get; set;}
}