namespace Cinnamon.Api.Data.Models.Activity.Request;

public class GetAllActivities
{
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}