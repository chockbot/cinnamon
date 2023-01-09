using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetWaitListByGuidArgs
{
    [Required]
    public string Guid { get; set; }    
}
