namespace Cinnamon.Api.Data.Models.ResendEmail.Request;

public class UpdateResendEmailArgs
{
    public int ResendEmailId { get; set; }
    public string? Email { get; set; }
    public DateTime? DateResend { get; set; }
}