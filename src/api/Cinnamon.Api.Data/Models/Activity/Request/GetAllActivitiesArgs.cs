namespace Cinnamon.Api.Data.Models.Activity.Request;

public class GetAllActivitiesArgs
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}