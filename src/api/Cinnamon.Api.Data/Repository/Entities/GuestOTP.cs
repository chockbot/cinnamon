namespace Cinnamon.Api.Data.Repository.Entities;
public class GuestOTP : BaseEntity
{
    public string Email { get; set; }
    public string OTPCode { get; set; }
}
