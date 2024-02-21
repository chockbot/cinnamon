using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class GenerateUnreadChatsNotificationJob : IJob
{
    private readonly INotifyUnreadChatsHandler notifyUnreadChatsHandler;

    public GenerateUnreadChatsNotificationJob(INotifyUnreadChatsHandler notifyUnreadChatsHandler)
    {
        this.notifyUnreadChatsHandler = notifyUnreadChatsHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return notifyUnreadChatsHandler.ExecuteAsync(new ChatService.Interactors.NotifyUnreadChatsArgs {});
    }
}