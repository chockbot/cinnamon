using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class SendVerifyEmailArgs : IInteractor 
{
    public string Email {get; set;}
    public string VerificationLink {get; set;}
}