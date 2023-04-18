using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.FailedLogin.Request;

public class GetFailedLoginsArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    // date format must yyyyMMddHHmmss
    [Required]
    public string DateFrom { get; set; }
    // date format must yyyyMMddHHmmss
    [Required]
    public string DateTo { get; set; }
}