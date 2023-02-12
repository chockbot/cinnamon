namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
public class CreateStudentAttendanceResult
{
    public int StudentId { get; set; }
    public bool IsPresent { get; set; }
    public DateTime AttendanceDate { get; set; }
}
