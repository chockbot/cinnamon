using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class OteBookedCountArgs
{
    [Required]
    public int ActivityId {get; set;}
}
