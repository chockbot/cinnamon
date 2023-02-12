using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class CreateStudentAttendanceArgs
{
    [Required]
    public int StudentId { get; set; }
    [Required]
    public bool IsPresent { get; set; }
    [Required]
    public DateTime AttendanceDate { get; set; }
}
