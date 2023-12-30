namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class PopularActivitiesArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CategoryId {get; set;}
}