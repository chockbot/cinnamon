using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;

public class CreateFailedLoginArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    public DateTime LoginDate {get; set;}
    public string? Metadata {get; set;}
}