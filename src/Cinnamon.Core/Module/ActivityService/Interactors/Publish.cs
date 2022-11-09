using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.ActivityService.Interactors;

public class Publish : IInteractor
{
    public bool IsPublished { get; set; }
    public int ActivityId { get; set; }
}