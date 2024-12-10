namespace Cinnamon.Web.Models.Account;

public class OTPModel
{
    public string Email { get; set; }
    public string OTPCode { get; set; }
    public DateTime CreatedOn { get; set; }
}
