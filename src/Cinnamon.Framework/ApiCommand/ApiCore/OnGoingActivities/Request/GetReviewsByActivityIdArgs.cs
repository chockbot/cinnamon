using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetReviewsByActivityIdArgs
{
    [Required]
    public int ActivityId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
