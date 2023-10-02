using System.ComponentModel.DataAnnotations;
namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;

public class GetEnrolleeMasterListArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    [Required]
    public int ProviderId { get; set; }
}
