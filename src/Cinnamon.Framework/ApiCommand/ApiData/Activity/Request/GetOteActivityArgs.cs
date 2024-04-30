namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetOteActivityArgs
{
    public bool? IncludeDescription {get; set;}
    public bool? IncludeAddress {get; set;}
    public bool? IncludeSchedule {get; set;}
    public bool? IncludePricing {get; set;}
    public bool? IncludeImages {get; set;}
    public bool? IncludeProvider {get; set;}
    public bool? IncludeOnlineEvents { get; set;}
    public bool? IncludeTickets { get; set; }
}