using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

public class RemoveActivityImageArgs
{
    [Required]
    public int ActivityId {get; set;}
}