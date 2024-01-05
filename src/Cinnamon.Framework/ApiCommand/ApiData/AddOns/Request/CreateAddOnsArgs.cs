using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;

public class CreateAddOnsArgs
{
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public IEnumerable<AddOn> AddOns { get; set; }
    public class AddOn
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string UnitPrice { get; set; } = "PHP";
        public string Description { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
