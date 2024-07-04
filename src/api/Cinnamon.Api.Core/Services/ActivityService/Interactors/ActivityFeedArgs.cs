using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class ActivityFeedArgs : IInteractor
{
    public int Skip {get; set;}
    public int Take {get; set;}
    public string? Search {get; set;}
    public int? CategoryId {get; set;}
    public int? StarReview {get; set;}
    public int? ExperienceType {get; set;}
    public int? ExperienceCategory {get; set;}
}