namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class GetAllStudentAttendanceByIdArgs
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    // date format must yyyyMMdd
    public string? Date { get; set; }
    public bool? IsIncludeStudent { get; set; }
    
    public IEnumerable<int>? ScheduleIds { get; set; }
}
