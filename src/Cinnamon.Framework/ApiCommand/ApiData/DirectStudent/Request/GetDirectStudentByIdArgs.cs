namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class GetDirectStudentByIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeStudent { get; set; }
    public IEnumerable<int>? ActivityIds { get; set; }
    public IEnumerable<int>? ScheduleIds { get; set; }
    public int? StudentId { get; set; }
}
