namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
public class GetMakerActivitiesArgs
{
    public int CustomerId { get; set; }
    public bool? IsActive { get; set; }
    public bool IncludeActivityDescription { get; set; }
    public bool? IncludeStudents { get; set; }
}
