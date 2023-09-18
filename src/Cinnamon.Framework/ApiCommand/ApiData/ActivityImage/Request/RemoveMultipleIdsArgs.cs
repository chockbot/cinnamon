using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.ActivityImage.Request;

public class RemoveMultipleIdsArgs
{
    [Required]
    public IList<int> Ids {get; set;}
}