namespace Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;

public class GetSeatPlanTemplateArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public string? TemplateName { get; set; }
}