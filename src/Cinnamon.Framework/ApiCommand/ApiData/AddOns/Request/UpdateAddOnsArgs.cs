using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;

public class UpdateAddOnsArgs
{
    [Required]
    public IEnumerable<UpdateAddOn> AddOns { get; set; }
    public class UpdateAddOn
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ActivityId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? UnitPrice { get; set; } = "PHP";
        public string? Description { get; set; }
        public int? Order { get; set; }
    }
}
