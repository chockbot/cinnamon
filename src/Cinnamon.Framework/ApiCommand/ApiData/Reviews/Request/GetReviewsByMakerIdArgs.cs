namespace Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;
public class GetReviewsByMakerIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? MakerId { get; set; }
}
