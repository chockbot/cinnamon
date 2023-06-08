namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;

public class GetReviewsByCustomerIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId { get; set; }
}
