using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class GetDirectStudentByIdArgs
{
    [Required]
    public int StudentId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
}
