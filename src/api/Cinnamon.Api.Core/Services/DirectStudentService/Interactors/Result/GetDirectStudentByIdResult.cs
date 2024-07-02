namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Result;
public class GetDirectStudentByIdResult
{
    public IEnumerable<StudentAttendace> StudentAttendaces { get; set; }
    public class StudentAttendace
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public int NumberOfSessions { get; set; }
        public int SessionsAttended { get; set; }
        public string Status { get; set; }
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityDescription { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleTitle { get; set; }
        public string ScheduleDescription { get; set; }
        public bool IsPresent { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Remarks { get; set; }
        public int NumberOfBackTracking { get; set; }
    }
}
