using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class ChangeEmailArgs
{
    [Required]
    [EmailAddress]
    public string CurrentEmail { get; set; }
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
}
