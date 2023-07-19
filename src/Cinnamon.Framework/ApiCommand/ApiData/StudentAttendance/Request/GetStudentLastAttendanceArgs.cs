using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class GetStudentLastAttendanceArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
}
