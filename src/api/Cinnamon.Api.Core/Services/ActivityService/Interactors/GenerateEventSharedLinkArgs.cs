using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class GenerateEventSharedLinkArgs : IInteractor
{
    public string Handler {get; set;}
    public int DateId {get; set;}
}