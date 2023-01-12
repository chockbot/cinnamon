using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class CheckCustomerLoginArgs
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    public string Password {get; set;}
}