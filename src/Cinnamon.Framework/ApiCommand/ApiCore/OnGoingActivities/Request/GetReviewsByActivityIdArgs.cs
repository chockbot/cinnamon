using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetReviewsByActivityIdArgs
{
    [Required]
    public int ActivityId { get; set; }
}
