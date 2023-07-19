namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;

public class GetStudentLastAttendanceResult
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public bool IsPresent { get; set; }
    public DateTime AttendanceDate { get; set; }
    public int ScheduleId { get; set; }
    public int ActivityId { get; set; }
}
