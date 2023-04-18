namespace Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Request;

public class GetAllOngoingActivityArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? CustomerId {get; set;}
    public bool? IsIncludeActivity {get; set;}
}