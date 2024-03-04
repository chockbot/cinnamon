using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class EventBuyerPoliciesArgs 
{
    [Required]
    public string Content {get; set;}
}