using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.Waitlist.Request;

public class UpdateWaitlistArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string? Guid { get; set; }
    public string? Token { get; set; }
    public bool? IsVerified { get; set; }
}