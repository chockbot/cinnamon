using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;

public class DeleteAddOnsArgs
{
    [Required]
    public IEnumerable<int> AddOnsId { get; set; }
}
