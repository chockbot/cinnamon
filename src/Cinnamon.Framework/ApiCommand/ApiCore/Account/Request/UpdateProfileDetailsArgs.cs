using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class UpdateProfileDetailsArgs 
{
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public DateTime? Datebirth {get; set;}
    public string? About {get; set;}
    public int VerifiedBadge { get; set; }
    public int CustomerId { get; set; }
    public bool IsOG { get; set; }
    public bool IsOF { get; set; }
}