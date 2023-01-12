using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;

public class UpdateWaitlistArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string? Guid { get; set; }
    public string? Token { get; set; }
    public bool? IsVerified { get; set; }
}