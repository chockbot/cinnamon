using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
public class GetStudentAttendanceArgs
{
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
}
