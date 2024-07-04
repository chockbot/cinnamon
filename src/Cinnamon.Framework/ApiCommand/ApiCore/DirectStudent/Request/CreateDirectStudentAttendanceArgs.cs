using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class CreateDirectStudentAttendanceArgs
{
    [Required]
    public IEnumerable<CreateStudentAttendance> CreateStudentAttendances { get; set; }

    public class CreateStudentAttendance
    {
        [Required]
        public int DirectStudentSessionId { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
