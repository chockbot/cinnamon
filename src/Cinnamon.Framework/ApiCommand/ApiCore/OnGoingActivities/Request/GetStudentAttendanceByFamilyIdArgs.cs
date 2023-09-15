using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetStudentAttendanceByFamilyIdArgs
{
    [Required]
    public int FamilyId { get; set; }
}
