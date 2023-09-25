namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class GetEnrolleeMasterListArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int ProviderId { get; set; }
}
