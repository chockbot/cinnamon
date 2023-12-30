using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;

public class DeleteAddOnArgs
{
    [Required]
    public int AddOnId { get; set; }
}
