using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetReviewsByMakerIdArgs
{
    [Required]
    public int MakerId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
