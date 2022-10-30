using System;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.NotificationService.Interactors;

public class WelcomeNotification : IInteractor 
{
    public string Email { get; set; }
}