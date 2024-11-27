namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
public class SendOTPResult
{
    public string Email { get; set; }
    public int[] OTPCode { get; set; }
}
