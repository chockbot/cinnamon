namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
public class VerifyOTPResult
{
    public string Email { get; set; }
    public string OTPCode { get; set; }
    public DateTime CreatedOn { get; set; }
}
