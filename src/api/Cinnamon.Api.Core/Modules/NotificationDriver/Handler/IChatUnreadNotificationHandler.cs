using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Handler;

public interface IChatUnreadNotificationHandler : IInteractorHandler<ChatUnreadNotificationArgs, AppResult<ChatUnreadNotificationResult>>
{
}