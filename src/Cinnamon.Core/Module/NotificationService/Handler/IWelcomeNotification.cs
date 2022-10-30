using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Interactors.Results;

namespace Cinnamon.Core.Module.NotificationService.Handler;

public interface IWelcomeNotification : IInteractorHandler<WelcomeNotification, AppResult<WelcomeNotificationResult>>
{
}