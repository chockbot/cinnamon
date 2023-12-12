namespace Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;

public class UpdateAddOnArgs
{
    public int Id { get; set; }
    public int? ActivityId { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? UnitPrice { get; set; } = "PHP";
    public string? Description { get; set; }
    public int? Order { get; set; }
}
