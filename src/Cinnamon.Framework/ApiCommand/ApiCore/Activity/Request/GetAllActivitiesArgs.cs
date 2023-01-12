using Cinnamon.Framework.Interactor;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class GetAllActivitiesArgs
{
    public bool? IncludeAtivitySchedules { get; set; }
    public bool? IncludeActivityAddress { get; set; }
    public bool? IncludeActivityDescription { get; set; }
    public bool? IncludeActivitySearchTags { get; set; }
    public bool? IncludeActivityImages { get; set; }
    public bool? IsActive { get; set; }
}
