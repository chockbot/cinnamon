using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class OteScheduleDatesArgs
{
    [Required]
    public int ActivityId {get; set;}

    public string? From {get; set;}

    public string? To {get; set;}
}
