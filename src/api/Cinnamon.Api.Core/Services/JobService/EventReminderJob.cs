using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class EventReminderJob : IJob 
{
    private readonly IOteEmailReminderHandler emailReminderHandler;

    public EventReminderJob(IOteEmailReminderHandler emailReminderHandler)
    {
        this.emailReminderHandler = emailReminderHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return emailReminderHandler.ExecuteAsync(new());
    }
}