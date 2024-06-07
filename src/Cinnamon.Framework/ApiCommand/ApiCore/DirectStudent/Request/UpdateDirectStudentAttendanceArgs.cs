using Cinnamon.Framework.Enums;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class UpdateDirectStudentAttendanceArgs
{
    public DateTime Date { get; set; }
    public IEnumerable<StudentDetails> Students { get; set; }

    public class StudentDetails
    {
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public bool IsPresent { get; set; }
    }
}
