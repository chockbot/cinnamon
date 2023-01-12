namespace Cinnamon.Web.Models.Entities;
public class ActivityAddress
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string District { get; set; }
    public string City { get; set; }
}
