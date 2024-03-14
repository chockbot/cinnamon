using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteSharedLinkVerificationArgs : IInteractor 
{
    public string Guid {get; set;}
    public string Token {get; set;}
    public string QrCode {get; set;}
}