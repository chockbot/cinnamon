using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UpdateProfileDetailsArgs 
{
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? Email { get; set; }
    public DateTime? Datebirth {get; set;}
    public string? About {get; set;}
    public string? PhoneNumber { get; set; }
    public int VerifiedBadge { get; set; }
    public DateTime? VerifiedBadgeDate { get; set; }
    public int CustomerId { get; set; }
    public bool IsOG { get; set; }
    public DateTime? IsOGDate { get; set; }
    public bool IsOF { get; set; }
    public DateTime? IsOFDate { get; set; }
}