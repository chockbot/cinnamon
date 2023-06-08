namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;

public class GetReviewsByActivityIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? ActivityId { get; set; }
}
