namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;

public class GetAllReviewsArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}