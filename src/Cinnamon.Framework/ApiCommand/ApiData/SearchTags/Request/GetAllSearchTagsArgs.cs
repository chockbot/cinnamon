namespace Cinnamon.Framework.ApiCommand.ApiData.SearchTags.Request;

public class GetAllSearchTagsArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
