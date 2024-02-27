using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteVerificationArgs : IInteractor 
{
    public string Handler {get; set;}
    public string QrCode {get; set;}
    public int DateId {get; set;}
}