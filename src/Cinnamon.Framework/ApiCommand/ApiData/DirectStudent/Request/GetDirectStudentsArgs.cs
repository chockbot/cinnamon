namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class GetDirectStudentsArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? ActivityId {get; set;}
    public int? ScheduleId {get; set;}
    public string? Status {get; set;}
}