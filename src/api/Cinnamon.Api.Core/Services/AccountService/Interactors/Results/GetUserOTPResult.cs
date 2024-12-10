namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
public class GetUserOTPResult
{
    public IEnumerable<GuestOTP> GuestOTPs { get; set; }
    public class GuestOTP
    {
        public string Email { get; set; }
        public string OTPCode { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
