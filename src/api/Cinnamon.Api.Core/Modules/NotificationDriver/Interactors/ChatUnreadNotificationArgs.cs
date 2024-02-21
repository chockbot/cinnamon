using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class ChatUnreadNotificationArgs : IInteractor 
{
    public IEnumerable<string> Emails {get; set;}
}