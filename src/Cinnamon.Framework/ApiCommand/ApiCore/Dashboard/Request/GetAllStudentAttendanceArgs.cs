using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetAllStudentAttendanceArgs
{
    [Required]
    public int ActivityId { get; set; }
}
