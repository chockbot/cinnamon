using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteValidateSharedLinkArgs : IInteractor
{
    public string Guid {get; set;}
    public string Token {get; set;}
}