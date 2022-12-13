namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetAllActivities
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}