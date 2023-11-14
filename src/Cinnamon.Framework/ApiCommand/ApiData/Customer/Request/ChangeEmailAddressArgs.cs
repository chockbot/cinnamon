using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Customer.Request;

public class ChangeEmailAddressArgs { 
    [Required]
    [EmailAddress]
    public string CurrentEmail { get; set; }
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
}
