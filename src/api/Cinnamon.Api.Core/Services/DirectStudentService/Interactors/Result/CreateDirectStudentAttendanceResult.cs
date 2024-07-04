namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
public class CreateDirectStudentAttendanceResult
{
    public IEnumerable<CreateStudentAttendance> CreateDirectStudentsAttendance { get; set; }

    public class CreateStudentAttendance
    {
        public int DirectStudentSessionId { get; set; }
        public bool IsPresent { get; set; }
        public DateTime Date { get; set; }
    }
}
