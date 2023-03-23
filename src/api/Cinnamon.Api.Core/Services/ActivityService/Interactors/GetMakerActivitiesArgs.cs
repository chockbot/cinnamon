using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;
public class GetMakerActivitiesArgs : IInteractor
{
    public int CustomerId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IncludeActivityDescription { get; set; }
    public bool? IncludeStudents { get; set; }
}
