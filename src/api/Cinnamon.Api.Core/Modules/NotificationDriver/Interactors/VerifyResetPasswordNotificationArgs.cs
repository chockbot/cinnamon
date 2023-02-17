using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class VerifyResetPasswordNotificationArgs : IInteractor 
{
    public string Email {get; set;}
    public DateTime DateChanged {get; set;}
}