using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class PopularActivitiesArgs : IInteractor
{
    public int? Take {get; set;}
    public int? Skip {get; set;}
    public int? CategoryId {get; set;}
}