using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class GenerateResetPasswordTokenArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
}