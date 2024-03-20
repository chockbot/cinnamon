using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteUpdateSharedLinkStatusArgs : IInteractor
{
    public string Guid {get; set;}
    public string Token {get; set;}
    public bool Enable {get; set;}
}