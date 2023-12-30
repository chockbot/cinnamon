using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;
public class DeleteAddOnsArgs : IInteractor
{
    public IEnumerable<int> AddOnIds { get; set; }
}
