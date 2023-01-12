namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetAllActivities
{
    public int? CustomerId {get; set;}
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeAddress {get; set;}
    public bool? IncludeDescription {get; set;}
    public bool? IncludeSearchTags {get; set;}
    public bool? IncludeSchedules {get; set;}
    public bool? IncludeImages {get; set;}
}