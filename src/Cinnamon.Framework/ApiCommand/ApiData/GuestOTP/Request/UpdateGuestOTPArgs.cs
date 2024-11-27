using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;

public class UpdateGuestOTPArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string OTPCode { get; set; }
}
