using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.GuestOTP.Request;

public class GetGuestOTPByEmailArgs
{
    [Required] 
    public string Email { get; set; }
}
