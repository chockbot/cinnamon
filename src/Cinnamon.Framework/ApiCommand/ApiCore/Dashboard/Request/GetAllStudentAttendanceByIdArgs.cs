using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetAllStudentAttendanceByIdArgs
{
    [Required]
    public int StudentId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
}
