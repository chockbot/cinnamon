using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;

public class CreateWaitlistArgs
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Guid { get; set; }
    [Required]
    public string Token { get; set; }
    public bool IsVerified { get; set; } = false;
}