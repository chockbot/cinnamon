namespace Cinnamon.Api.Data.Repository.Entities;
public class AddOns : BaseEntity
{
    public int ActivityId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string UnitPrice { get; set; } = "PHP";
    public string Description { get; set; }
    public int Order { get; set; }
}
