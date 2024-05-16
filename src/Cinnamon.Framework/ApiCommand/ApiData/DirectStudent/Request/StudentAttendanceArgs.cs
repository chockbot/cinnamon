namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class StudentAttendanceArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    // date format must yyyyMMdd
    public string? Date { get; set; }
    public bool? IsIncludeStudent {get; set;}
    public IEnumerable<int>? ActivityIds {get; set;}
    public IEnumerable<int>? ScheduleIds {get; set;}
}