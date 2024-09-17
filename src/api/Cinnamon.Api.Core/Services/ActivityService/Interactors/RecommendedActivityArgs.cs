using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class RecommendedActivityArgs : IInteractor
{
    public int Count { get; set; }
    public bool? IncludeOteSchedule { get; set; }
}