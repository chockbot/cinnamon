using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class EventThankYouJob : IJob 
{
    private readonly IOteEmailThankYouHandler emailThankYouHandler;

    public EventThankYouJob(IOteEmailThankYouHandler emailThankYouHandler)
    {
        this.emailThankYouHandler = emailThankYouHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return emailThankYouHandler.ExecuteAsync(new());
    }
}