using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetAllStudentsByIdArgs
{
    [Required]
    public int CustomerId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
