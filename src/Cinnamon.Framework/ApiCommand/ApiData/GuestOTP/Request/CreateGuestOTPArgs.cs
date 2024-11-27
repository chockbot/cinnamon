using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;

public class CreateGuestOTPArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string OTECode { get; set; }
}
