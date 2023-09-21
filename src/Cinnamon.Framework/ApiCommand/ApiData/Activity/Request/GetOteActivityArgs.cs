namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetOteActivityArgs
{
    public bool? IncludeDescription {get; set;}
    public bool? IncludeAddress {get; set;}
    public bool? IncludeSchedule {get; set;}
    public bool? IncludePricing {get; set;}
}