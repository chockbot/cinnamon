using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class ApprovedWaitListArgs
{
    [Required]
    public int WaitListId {get; set;}
}
