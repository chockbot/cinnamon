using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ResendEmail.Request;

public class CreateEmailResendArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public DateTime DateResend { get; set; } = DateTime.Now;
}