using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.ResendEmail.Request;

public class GetResendEmailByEmailDateRangeArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    // date format must yyyyMMddHHmmss
    [Required]
    public string DateFrom { get; set; }
    // date format must yyyyMMddHHmmss
    [Required]
    public string DateTo { get; set; }
}