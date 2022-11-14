using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors;

public class SubmitUpdatedActivity : IInteractor 
{
    public ActivityModel Activity { get; set; }
    public IList<Tuple<int,byte[],string>> Images { get; set; }
    public IList<Tuple<int,string>> SearchTags { get; set; }
}