namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class GetAllStudentAttendanceArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    // date format must yyyyMMdd
    public string? Date { get; set; }
    public bool? IsIncludeStudent {get; set;}
    public int? ActivityId {get; set;}
    public int? ScheduleId {get; set;}
}