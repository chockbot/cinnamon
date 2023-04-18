namespace Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;

public class GetAllBadgeArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
