using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class PrivacyPoliciesArgs 
{
    [Required]
    public string Title {get; set;}

    [Required]
    public string Content {get; set;}
}