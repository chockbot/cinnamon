using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetCustomerByEmailArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }   
}
