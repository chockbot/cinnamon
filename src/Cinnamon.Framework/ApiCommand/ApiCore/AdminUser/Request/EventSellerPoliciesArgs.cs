using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class EventSellerPoliciesArgs 
{
    [Required]
    public string Content {get; set;}
}