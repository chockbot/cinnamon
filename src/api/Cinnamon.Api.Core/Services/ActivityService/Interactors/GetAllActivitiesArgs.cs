using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GetAllActivitiesArgs:IInteractor
{
    public bool IncludeAtivitySchedules { get; set; }
    public bool IncludeActivityAddress { get; set; }
    public bool IncludeActivityDescription { get; set; }
    public bool IncludeActivitySearchTags { get; set; }
    public bool IncludeActivityImages { get; set; }
    public bool IncludeExperienceTypes { get; set; }
    public bool? IncludeExperienceCategories { get; set; }
    public bool? IncludeSubCategories { get; set; }
    public bool? IsActive { get; set; }
}
