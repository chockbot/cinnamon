using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class SendOTPArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public int[] OTPCode { get; set; }
}
