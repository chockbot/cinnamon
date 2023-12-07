using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
public class CreateAddOnArgs
{
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string UnitPrice { get; set; } = "PHP";
    [Required]
    public string Description { get; set; }
    [Required]
    public int Order { get; set; }
}
