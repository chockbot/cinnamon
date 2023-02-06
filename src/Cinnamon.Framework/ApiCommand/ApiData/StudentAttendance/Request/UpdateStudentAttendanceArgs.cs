using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class UpdateStudentAttendanceArgs 
{
    [Required]
    public int AttendanceId {get; set;}
    public bool? IsPresent {get; set;}
    public DateTime? Date {get; set;}
}