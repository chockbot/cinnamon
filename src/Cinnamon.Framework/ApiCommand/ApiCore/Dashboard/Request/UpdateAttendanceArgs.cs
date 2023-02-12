using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class UpdateAttendanceArgs
{
    [Required]
    public int Id { get; set; }
    public bool IsPresent { get; set; }
    public DateTime Date { get; set; }
}
