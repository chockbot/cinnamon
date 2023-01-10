namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;
public class GetActivitiesByCategoriesArgs
{
    public bool? IncludeAtivitySchedules { get; set; }
    public bool? IncludeActivityAddress { get; set; }
    public bool? IncludeActivityDescription { get; set; }
    public bool? IncludeActivitySearchTags { get; set; }
    public bool? IncludeActivityImages { get; set; }
    public bool? IsActive { get; set; }
}
