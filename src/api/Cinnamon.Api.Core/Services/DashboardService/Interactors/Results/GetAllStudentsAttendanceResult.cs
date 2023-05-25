namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
public class GetAllStudentsAttendanceResult
{
    public IEnumerable<StudentAttendace> StudentAttendaces { get; set; }
    public class StudentAttendace
    {
        public int Id { get; set; }
        public int NumberOfSessions { get; set; }
        public int SessionsAttended { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
