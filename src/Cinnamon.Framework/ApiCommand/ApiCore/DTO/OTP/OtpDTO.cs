namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.OTP;

public class OtpDTO
{
    public string Email { get; set; }
    public int[] OTPcode { get; set; }
    public DateTime CreatedOn { get; set; }
    public string OtpCode { get; set; }
}
