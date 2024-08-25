namespace Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;

public class GetTemplatesArgs 
{
    public string? Name { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}