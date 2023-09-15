using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class GetStudentAttendanceByFamilyIdArgs
{
    [Required]
    public int familyId { get; set; }
}
