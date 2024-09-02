namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetActivityArgs
{
    public int? CustomerId {get; set;}
    public bool? IsActive { get; set; }
    public bool? IncludeAddress {get; set;}
    public bool? IncludeDescription {get; set;}
    public bool? IncludeSearchTags {get; set;}
    public bool? IncludeSchedules {get; set;}
    public bool? IncludeImages {get; set;}
    public bool? IncludeCustomer {get; set;}
    public bool? IncludeStudents { get; set; }
    public bool? IncludeTickets { get; set; }
    public bool? IncludeAddOns { get; set; }
    public bool? IncludeOteSchedule { get; set;}
}