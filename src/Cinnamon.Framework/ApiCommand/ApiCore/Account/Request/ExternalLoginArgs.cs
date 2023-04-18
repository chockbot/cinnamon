using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class ExternalLoginArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
}